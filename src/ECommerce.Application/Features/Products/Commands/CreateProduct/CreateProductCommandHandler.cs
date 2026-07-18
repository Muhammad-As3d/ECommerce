using ECommerce.Application.Interfaces.Services;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler(IUnitOfWork unitOfWork, IFileService fileService) : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categoryIsExist = await _unitOfWork.Repository<Category>().AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (!categoryIsExist)
            return Result.Failure<Guid>(CategoryErrors.NotFound(request.CategoryId));

        var product = Product.Create(
             request.Name,
             request.Description,
             request.Stock,
             request.ModelYear,
             request.Price,
             request.CategoryId
             );

        var imagesPaths = await fileService.UploadManyImageAsync(request.Images, cancellationToken);

        product.AddImages(imagesPaths);

        await _unitOfWork.Repository<Product>().AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product.Id);
    }
}