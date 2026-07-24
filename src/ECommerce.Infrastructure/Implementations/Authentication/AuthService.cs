using System.Security.Cryptography;

namespace ECommerce.Infrastructure.Implementations.Authentication;

public class AuthService(UserManager<ApplicationUser> userManager, ILogger<AuthService> logger,
    IEmailSender emailSender, IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider, ApplicationDbContext context) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpirationDays = 14;

    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (emailIsExists)
            return Result.Failure(UserErrors.DuplicatedEmail);

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return result.ToFailureIdentityResult();

        await SendConfirmationEmailAsync(user);

        return Result.Success();
    }

    public async Task<Result> ConfirmationEmailAsync(ConfirmationEmailRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailIsConfirmed);

        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (!result.Succeeded)
            return result.ToFailureIdentityResult();

        await _userManager.AddToRoleAsync(user, DefaultRoles.Customer.Name);

        return Result.Success();
    }

    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailIsConfirmed);

        await SendConfirmationEmailAsync(user);

        return Result.Success();
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.IsDisabled);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

        if (!result.Succeeded)
        {
            var error = result.IsNotAllowed
                ? UserErrors.EmailIsNotConfirmed
                : result.IsLockedOut
                ? UserErrors.LockedUser
                : UserErrors.InvalidCredentials;

            return Result.Failure<AuthResponse>(error);
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var (token, expiresIn) = _jwtProvider.GenerateTokenAsync(user, userRoles);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration,
            UserId = user.Id,
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, token, expiresIn, refreshToken, refreshTokenExpiration);

        return Result.Success(response);
    }

    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        var user = await _userManager.Users.Where(x => x.Id == userId).Include(x => x.RefreshTokens).FirstOrDefaultAsync();

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.IsDisabled);

        var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        if (user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthResponse>(UserErrors.LockedUser);

        await context.RefreshTokens
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedOn, DateTime.UtcNow));

        var userRoles = await _userManager.GetRolesAsync(user);

        var (newToken, expiresIn) = _jwtProvider.GenerateTokenAsync(user, userRoles);
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
            return Result.Failure(UserErrors.InvalidToken);

        var user = await _userManager.Users.Where(x => x.Id == userId).Include(x => x.RefreshTokens).FirstOrDefaultAsync();

        if (user is null)
            return Result.Failure(UserErrors.InvalidToken);

        var userRefreshToken = user.RefreshTokens.FirstOrDefault(c => c.Token == refreshToken && c.IsActive);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidToken);

        await context.RefreshTokens
            .Where(x => x.UserId == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedOn, DateTime.UtcNow));

        return Result.Success();
    }

    private async Task SendConfirmationEmailAsync(ApplicationUser user)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Confirmation  {Code} => ", code);

        var route = "Auth/confirmation-email";

        var origin = _httpContextAccessor.HttpContext!.Request.Headers.Origin;

        var Url = $"{origin}/{route}?userId={user.Id}&code={code}";

        await _emailSender.SendEmailAsync(user.Email!, "Confirmation Email", Url);
    }

    private static string GenerateRefreshToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}