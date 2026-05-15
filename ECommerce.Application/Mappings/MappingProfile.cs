using AutoMapper;
using ECommerce.Application.Contracts.Category;
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

        //Authentication
        //CreateMap<RegisterRequest, ApplicationUser>


        //Product Mappings
        CreateMap<Product, ProductResponse>();
        //CreateMap<Product, ProductResponse>().ReverseMap();

    }
}
