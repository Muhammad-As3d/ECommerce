using AutoMapper;
using ECommerce.Api.ViewModels.Products;
using ECommerce.Application.Contracts.Products;
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

    }
}
