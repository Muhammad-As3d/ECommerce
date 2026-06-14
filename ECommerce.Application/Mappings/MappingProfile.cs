using AutoMapper;
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
    }
}