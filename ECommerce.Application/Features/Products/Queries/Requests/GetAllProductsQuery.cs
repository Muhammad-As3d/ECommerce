using AutoMapper;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries.Requests;

public record GetAllProductsQuery() : IRequest<IEnumerable<GetProductDto>>;

public class GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllProductsQuery, IEnumerable<GetProductDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<GetProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Repository<Product>().GetAllAsync();

        return _mapper.Map<IEnumerable<GetProductDto>>(products);
    }
}
