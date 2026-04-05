using AutoMapper;
using ECommerce.Application.Abstractions.Errors;
using ECommerce.Application.Abstractions.Results;
using ECommerce.Application.DTOs.Category;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Categories.Queries.Requests;

public record GetCategoryByIdQuery(int Id) : IRequest<Result<GetCategoryDto>>;

public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, Result<GetCategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<GetCategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            return Result.Failure<GetCategoryDto>(CategoryError.CategoryNotFound);

        return Result.Success(_mapper.Map<GetCategoryDto>(category));
    }
}