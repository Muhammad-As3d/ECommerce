using ECommerce.Application.Abstractions.Constants;

namespace ECommerce.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Request.CurrentPassword)
           .NotEmpty();

        RuleFor(x => x.Request.NewPassword)
           .NotEmpty()
           .Matches(RegexPattern.Password)
           .WithMessage("password should be at least 8 digits and contains upperCase, lowercase, NonAlphanumeric")
           .NotEqual(x => x.Request.CurrentPassword)
           .WithMessage("new password cannot be same as current password.");
    }
}
