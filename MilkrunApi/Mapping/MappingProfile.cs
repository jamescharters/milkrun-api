using AutoMapper;
using MilkrunApi.Models;
using MilkrunApi.Models.Entity;

namespace MilkrunApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductEntity, ProductRequest>().ReverseMap();
        CreateMap<ProductEntity, ProductResponse>().ReverseMap();
    }
}