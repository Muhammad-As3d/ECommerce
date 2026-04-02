using AutoMapper;
using ECommerce.Application.Abstractions.Errors;
using ECommerce.Application.Abstractions.Results;
using ECommerce.Application.Contracts.Category;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Categories.Queries.Requests;

public record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryResponse>>;

public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryError.CategoryNotFound);

        return Result.Success(_mapper.Map<CategoryResponse>(category));
    }
}