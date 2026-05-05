using AutoMapper;
using ECommerce.Application.Contracts.Authentication;
using ECommerce.Application.Contracts.Products;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Authentication
        //CreateMap<RegisterRequest, ApplicationUser>


        //Product Mappings
        CreateMap<GetProductDto, Product>();

        //Category Mappings
    }
}
