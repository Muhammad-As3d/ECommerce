namespace ECommerce.Infrastructure.Implementations.Authentication;

public class AuthService(UserManager<ApplicationUser> userManager, ILogger<AuthService> logger,
    IEmailSender emailSender, IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

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
            return result.ToFailureResult();

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
            return result.ToFailureResult();

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

        var (token, expiresIn) = await _jwtProvider.GenerateTokenAsync(user, userRoles);

        //TODO: Refresh Token

        var response = new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, token, expiresIn, "", DateTime.UtcNow);

        return Result.Success(response);
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
}
