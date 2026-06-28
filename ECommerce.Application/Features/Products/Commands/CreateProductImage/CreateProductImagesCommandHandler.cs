using ECommerce.Application.Services;

namespace ECommerce.Application.Features.Products.Commands.CreateProductImage;

internal class CreateProductImagesCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
    : IRequestHandler<CreateProductImagesCommand, Result>
{
    public async Task<Result> Handle(CreateProductImagesCommand request, CancellationToken cancellationToken)
    {
        var repo = unitOfWork.Repository<Product>();

        var isExists = await repo.AnyAsync(x => x.Id == request.ProductId, cancellationToken);

        if (!isExists)
            return Result.Failure(ProductErrors.NotFound(request.ProductId));

        var imagesUrls = await fileService.UploadManyImageAsync(request.Images, cancellationToken);

        var productImages = imagesUrls.Select(url => new ProductImage
        {
            ImageUrl = url,
            ProductId = request.ProductId
        }).ToList();

        await unitOfWork.Repository<ProductImage>().AddRangeAsync(productImages, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
