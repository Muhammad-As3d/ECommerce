using ECommerce.Application.Services;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct;

internal class CreateProductCommandHandler(IUnitOfWork unitOfWork, IFileService fileService) : IRequestHandler<CreateProductCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
             request.Name,
             request.Description,
             request.Stock,
             request.ModelYear,
             request.Price,
             request.categoryId
             );

        var imagesPaths = await fileService.UploadManyImageAsync(request.Images, cancellationToken);

        product.AddImages(imagesPaths);

        await _unitOfWork.Repository<Product>().AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(product.Id);
    }
}