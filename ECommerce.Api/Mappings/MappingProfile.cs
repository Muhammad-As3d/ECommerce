using AutoMapper;
using ECommerce.Api.Contracts.Category;
using ECommerce.Application.DTOs.Category;

namespace ECommerce.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GetCategoryDto, CategoryResponse>().ReverseMap();
    }
}
