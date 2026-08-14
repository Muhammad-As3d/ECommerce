namespace ECommerce.Application.Features.Addresses.Commands.AddAddress;

public class AddAddressCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<AddAddressCommand, Result>
{
    public async Task<Result> Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
        var address = Address.Create(currentUser.Id, currentUser.FullName, request.Street, request.City, request.Governorate,
             request.Country, request.PostalCode, request.PhoneNumber, request.IsDefault);

        await unitOfWork.Repository<Address>().AddAsync(address, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
