using ECommerce.Application.Interfaces;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Categories.Create;

public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {

        var category = Category.Create(request.Name, request.Description);

        await _unitOfWork
            .Repository<Category>()
            .AddAsync(category, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
