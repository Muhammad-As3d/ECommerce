namespace ECommerce.Application.Features.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<UpdateAddressCommand, Result>
{
    public async Task<Result> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var repo = unitOfWork.Repository<Address>();

        var addressId = await repo
            .GetByPredicateProjectAsync(x => x.UserId == currentUser.Id, x => (Guid?)x.Id, cancellationToken);

        if (addressId is null)
            return AddressErrors.AddressNotFound;

        var address = Address.CreateStub(addressId.Value);

        var changedProperties = address.Update(request.Street, request.City, request.Governorate,
            request.Country, request.PostalCode, request.PhoneNumber, request.IsDefault);

        repo.PartialUpdate(address, changedProperties);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
