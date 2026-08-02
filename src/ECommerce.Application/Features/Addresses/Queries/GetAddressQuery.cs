using ECommerce.Application.Contracts.Addresses;

namespace ECommerce.Application.Features.Addresses.Queries;

public class GetAddressQuery() : IRequest<Result<AddressUserResponse>>;

public class GetAddressQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetAddressQuery, Result<AddressUserResponse>>
{
    public async Task<Result<AddressUserResponse>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
    {
        var address = await unitOfWork
            .Repository<Address>()
            .GetByPredicateProjectAsync<AddressResponse>(x => x.UserId == currentUser.Id, cancellationToken);

        var response = new AddressUserResponse(currentUser.FullName, address!);

        return Result.Success(response);
    }
}