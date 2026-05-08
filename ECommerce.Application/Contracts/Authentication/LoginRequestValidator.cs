using ECommerce.Application.Abstractions.Constants;
using FluentValidation;

namespace ECommerce.Application.Contracts.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPattern.Password)
            .WithMessage("password should be at least 8 digits and contains upperCase, lowercase, NonAlphanumeric");
    }
}
