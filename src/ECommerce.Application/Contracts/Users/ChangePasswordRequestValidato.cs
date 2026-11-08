using ECommerce.Application.Abstractions.Constants;

namespace ECommerce.Application.Contracts.Users;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(RegexPattern.Password)
            .WithMessage("password should be at least 8 digits and contains upperCase, lowercase, NonAlphanumeric")
            .NotEqual(x => x.CurrentPassword);
    }
}
