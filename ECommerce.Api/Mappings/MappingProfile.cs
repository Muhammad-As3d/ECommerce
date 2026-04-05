using AutoMapper;
using ECommerce.Api.Contracts.Category;
using ECommerce.Api.Contracts.Products;
using ECommerce.Application.DTOs.Category;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Features.Categories.Commands.Requests;
using ECommerce.Application.Features.Products.Commands.CreateProduct;

namespace ECommerce.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product Mapping
        CreateMap<ProductRequest, CreateProductCommand>().ReverseMap();
        CreateMap<GetProductDto, ProductResponse>().ReverseMap();

        // Category Mapping
        CreateMap<GetCategoryDto, CategoryResponse>().ReverseMap();
        CreateMap<CreateCategoryRequest, CreateCategoryCommand>();
        CreateMap<GetCategoryDto, CategoryResponse>().ReverseMap();

    }
}
