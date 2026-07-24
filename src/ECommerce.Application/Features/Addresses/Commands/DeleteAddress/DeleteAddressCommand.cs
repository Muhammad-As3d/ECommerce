namespace ECommerce.Application.Features.Addresses.Commands.DeleteAddress;

public record DeleteAddressCommand() : IRequest<Result>;

public class DeleteAddressCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<DeleteAddressCommand, Result>
{
    public async Task<Result> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var rowsAffected = await unitOfWork
            .Repository<Address>()
            .DeleteAsync(x => x.UserId == currentUser.Id, cancellationToken);

        return rowsAffected > 0
            ? Result.Success()
            : Result.Failure(AddressErrors.AddressNotFound);
    }
}