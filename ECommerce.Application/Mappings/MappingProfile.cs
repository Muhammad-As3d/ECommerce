using AutoMapper;
using ECommerce.Application.Contracts.Products;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductResponse, Product>();
    }
}
