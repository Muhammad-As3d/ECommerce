using ECommerce.Application.Contracts.Products;
using ECommerce.Application.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Application.Features.Products.Queries.GetProduct;

public record GetProductByIdQuery(
    [property: FromRoute] int categoryId,
    int Id
    )
    : IRequest<Result<ProductResponse>>;


#region Validation
public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.categoryId)
            .SetValidator(new CategoryIdValidator(unitOfWork));
    }
}
#endregion