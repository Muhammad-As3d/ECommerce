using ECommerce.Application.Abstractions.Constants;
using FluentValidation;

namespace ECommerce.Application.Contracts.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .Length(3, 100)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .Length(3, 100)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPattern.Password)
            .WithMessage("password should be at least 8 digits and contains upperCase, lowercase, NonAlphanumeric");
    }
}
