using ECommerce.Application.Contracts.Category;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Categories.GetAll;

public record GetAllCategoriesQuery() : IRequest<IEnumerable<CategoryResponse>>;

public class GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<IEnumerable<CategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken) =>
        await _unitOfWork.Repository<Category>()
        .GetAllProjectionAsync<CategoryResponse>(cancellationToken);

}
