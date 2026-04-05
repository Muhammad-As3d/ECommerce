using ECommerce.Application.Features.Categories.Commands.Requests;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Categories.Commands.Handlers;

public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name,
            CreatedById = "88F56E41-4D68-4C35-914A-EBBA66691D5F",
            UpdatedById = "88F56E41-4D68-4C35-914A-EBBA66691D5F"
        };

        await _unitOfWork.Repository<Category>().AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);


        return Result.Success();
    }
}
