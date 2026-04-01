using AutoMapper;
using ECommerce.Application.Contracts.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries.Requests;

public record GetAllProductsQuery() : IRequest<IEnumerable<ProductResponse>>;

public class GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Repository<Product>().GetAllAsync();

        return _mapper.Map<IEnumerable<ProductResponse>>(products);
    }


}
