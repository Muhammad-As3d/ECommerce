namespace ECommerce.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.Request.FirstName)
            .NotEmpty()
            .Length(3, 10);

        RuleFor(x => x.Request.LastName)
            .NotEmpty()
            .Length(3, 10);
    }
}
