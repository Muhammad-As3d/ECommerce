using ECommerce.Application.Abstractions.Pagination;
using ECommerce.Application.Contracts.Products;
using ECommerce.Application.Shared;

namespace ECommerce.Application.Features.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(int CategoryId, SpecificationRequest Spec)
    : IRequest<PaginatedList<ProductResponse>>;


public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
{
    public GetAllProductsQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.CategoryId)
            .SetValidator(new CategoryIdValidator(unitOfWork));
    }
}
