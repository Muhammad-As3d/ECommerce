using AutoMapper;
using ECommerce.Application.Contracts.Category;
using ECommerce.Application.Contracts.Products;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Category
        CreateMap<Category, CategoryResponse>();

        #endregion

        //Authentication
        //CreateMap<RegisterRequest, ApplicationUser>


        //Product Mappings
        CreateMap<GetProductDto, Product>();

    }
}
