using AutoMapper;
using ECommerce.Application.Contracts.Category;
using ECommerce.Application.Contracts.Products;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Product Mappings
        CreateMap<ProductResponse, Product>();


        //Category Mappings
        CreateMap<Category, CategoryResponse>();
    }
}
