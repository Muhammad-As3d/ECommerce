namespace ECommerce.Application.Features.Products.Commands.ToggleStatus;

public class ToggleStatusProductCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ToggleStatusProductCommand, Result>
{
    public async Task<Result> Handle(ToggleStatusProductCommand request, CancellationToken cancellationToken)
    {
        var affectedRows = await unitOfWork
            .Repository<Product>()
            .ToggleStatusAsync(request.Id, cancellationToken);

        if (affectedRows == 0)
            return Result.Failure(ProductErrors.NotFound(request.Id));

        return Result.Success();
    }
}
