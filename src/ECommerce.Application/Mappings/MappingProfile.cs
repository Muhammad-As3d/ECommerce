using AutoMapper;
using ECommerce.Application.Contracts.Carts;
using ECommerce.Application.Contracts.Category;
using ECommerce.Application.Contracts.ProductImages;
using ECommerce.Application.Contracts.Products;

namespace ECommerce.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Category
        CreateMap<Category, CategoryResponse>();

        CreateMap<Category, CategoryProductsResponse>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        #endregion

        //Product Mappings
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.ImageURLs,
            opt => opt.MapFrom(src => src.ProductImages.Select(x => x.ImageUrl).ToList()));

        CreateMap<Product, ProductImagesResponse>()
            .ForMember(dest => dest.ImagesUrls,
            opt => opt.MapFrom(src => src.ProductImages.Select(x => x.ImageUrl).ToList()));

        CreateMap<ProductImage, ProductImageResponse>()
            .ConstructUsing(src => new ProductImageResponse(src.Id, src.ImageUrl));

        #region Cart
        CreateMap<CartItem, CartItemResponse>()
            .ConstructUsing(src =>
            new CartItemResponse(src.Id,
            src.ProductId,
            src.Product.Name,
            src.Product.ProductImages.Select(x => x.ImageUrl).FirstOrDefault() ?? string.Empty,
            src.UnitPriceSnapshot,
            src.Quantity,
            src.Quantity * src.UnitPriceSnapshot,
            src.Quantity < src.Product.Stock));

        CreateMap<Product, CartProductResponse>();

        CreateMap<Cart, CartResponse>()
            .ConstructUsing(src => new CartResponse(src.Id, src.CartItems.Count()));

        #endregion
    }
}