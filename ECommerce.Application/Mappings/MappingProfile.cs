using AutoMapper;
using ECommerce.Application.DTOs.Category;
using ECommerce.Application.DTOs.Products;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Product Mappings
        CreateMap<GetProductDto, Product>();

        //Category Mappings
        CreateMap<Category, GetCategoryDto>();
    }
}
