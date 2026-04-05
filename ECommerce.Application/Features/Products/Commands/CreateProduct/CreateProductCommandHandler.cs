using ECommerce.Application.Interfaces;
using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryIsExists = await _unitOfWork
            .Repository<Category>()
            .GetQueryable()
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryIsExists)
            return Result.Failure<int>(CategoryError.NotFound(request.CategoryId));

        var product = Product.Create(
             request.Name,
             request.Description,
             request.Stock,
             request.ModelYear,
             request.Price,
             request.CategoryId,
             "88F56E41-4D68-4C35-914A-EBBA66691D5F"
             );

        await _unitOfWork.Repository<Product>().AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product.Id);
    }
}
