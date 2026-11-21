using ECommerce.Application.Contracts.Users;
using ECommerce.Infrastructure.Helpers;
using ECommerce.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace ECommerce.Infrastructure.Implementations.Authentication;

public class AuthService(UserManager<User> userManager, IEmailSender emailSender,
    IOptions<AppSettings> appSettings, SignInManager<User> signInManager,
    IJwtProvider jwtProvider, ApplicationDbContext context, ILogger<AuthService> logger) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly AppSettings _appSettings = appSettings.Value;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpirationDays = 14;

    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (emailIsExists)
            return UserErrors.DuplicatedEmail;

        var user = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return result.ToError();

        await SendConfirmationEmailAsync(user);

        return Result.Success();
    }

    public async Task<Result> ConfirmationEmailAsync(ConfirmationEmailRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return UserErrors.InvalidCode;

        if (user.EmailConfirmed)
            return UserErrors.EmailAlreadyConfirmed;

        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return UserErrors.InvalidCode;
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (!result.Succeeded)
            return result.ToError();

        await _userManager.AddToRoleAsync(user, DefaultRoles.Customer.Name);

        return Result.Success();
    }

    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return UserErrors.EmailAlreadyConfirmed;

        await SendConfirmationEmailAsync(user);

        return Result.Success();
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return UserErrors.InvalidCredentials;

        if (user.IsDisabled)
            return UserErrors.IsDisabled;

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

        if (!result.Succeeded)
        {
            var error = result.IsNotAllowed
                ? UserErrors.EmailIsNotConfirmed
                : result.IsLockedOut
                ? UserErrors.LockedUser
                : UserErrors.InvalidCredentials;

            return error;
        }

        var (userRoles, userPermissions) = await GetUserRolesAndPermissionsAsync(user, cancellationToken);

        var (token, expiresIn) = _jwtProvider.GenerateTokenAsync(user, userRoles, userPermissions);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration,
            UserId = user.Id,
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

        return Result.Success(response);
    }

    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return UserErrors.InvalidToken;

        var user = await _userManager.Users.Where(x => x.Id == userId).Include(x => x.RefreshTokens).FirstOrDefaultAsync();

        if (user is null)
            return UserErrors.InvalidCredentials;

        if (user.IsDisabled)
            return UserErrors.IsDisabled;

        var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return UserErrors.InvalidToken;

        if (user.LockoutEnd > DateTime.UtcNow)
            return UserErrors.LockedUser;

        await context.RefreshTokens
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedOn, DateTime.UtcNow));

        var (roles, permissions) = await GetUserRolesAndPermissionsAsync(user, CancellationToken.None);

        var (newToken, expiresIn) = _jwtProvider.GenerateTokenAsync(user, roles, permissions);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration,
            UserId = userId,
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

        return Result.Success(response);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return UserErrors.InvalidToken;

        var user = await _userManager.Users.Where(x => x.Id == userId).Include(x => x.RefreshTokens).FirstOrDefaultAsync();

        if (user is null)
            return UserErrors.InvalidToken;

        var userRefreshToken = user.RefreshTokens.FirstOrDefault(c => c.Token == refreshToken && c.IsActive);

        if (userRefreshToken is null)
            return UserErrors.InvalidToken;

        await context.RefreshTokens
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedOn, DateTime.UtcNow));

        return Result.Success();
    }

    public async Task<Result> SendResetPasswordCodeAsync(string email)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return UserErrors.EmailIsNotConfirmed;

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        logger.LogInformation("Reset code => {code}", code);

        var url = $"{_appSettings.FrontendUrl.TrimEnd('/')}/auth/reset-password" +
                  $"?email={Uri.EscapeDataString(user.Email!)}&code={code}";

        var body = EmailBodyBuilder.Generate("ResetPassword", new Dictionary<string, string>
        {
            ["app_name"] = _appSettings.Name,
            ["name"] = user.FirstName,
            ["action_url"] = url,
            ["year"] = DateTime.UtcNow.Year.ToString()
        });

        await _emailSender.SendEmailAsync(user.Email!, $"Reset your {_appSettings.Name} password", body);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !user.EmailConfirmed)
            return UserErrors.InvalidCode;

        IdentityResult result;

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if (!result.Succeeded)
            return result.ToError();

        return Result.Success();
    }

    private async Task SendConfirmationEmailAsync(User user)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        logger.LogInformation("Confirmation code => {Code}", code);

        var url = $"{_appSettings.FrontendUrl.TrimEnd('/')}/auth/confirm-email" +
                  $"?userId={user.Id}&code={code}";

        var body = EmailBodyBuilder.Generate("EmailConfirmation", new Dictionary<string, string>
        {
            ["app_name"] = _appSettings.Name,
            ["name"] = user.FirstName,
            ["action_url"] = url,
            ["year"] = DateTime.UtcNow.Year.ToString()
        });

        await _emailSender.SendEmailAsync(user.Email!, $"Confirm your {_appSettings.Name} email", body);
    }

    private static string GenerateRefreshToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissionsAsync(User user, CancellationToken cancellationToken)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        //var userPermissions = await context.Roles
        //    .Join(context.RoleClaims, r => r.Id, rc => rc.RoleId, (r, rc) => new { r, rc })
        //    .Where(x => userRoles.Contains(x.r.Name!))
        //    .Select(x => x.rc.ClaimValue)
        //    .Distinct()
        //    .ToListAsync(cancellationToken: cancellationToken);

        var userPermissions = await (from r in context.Roles
                                     join rc in context.RoleClaims
                                     on r.Id equals rc.RoleId
                                     where userRoles.Contains(r.Name!)
                                     select rc.ClaimValue)
                                     .Distinct()
                                     .ToListAsync(cancellationToken);

        return (userRoles, userPermissions);
    }
}