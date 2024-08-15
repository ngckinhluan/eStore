using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.DTOs.Response;
using BusinessObjects.Entities;

namespace eStore.Extensions;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Product, ProductRequestDto>().ReverseMap();
        CreateMap<Product, ProductResponseDto>().ReverseMap();
        CreateMap<Member, MemberRequestDto>().ReverseMap();
        CreateMap<Member, MemberResponseDto>().ReverseMap();
        CreateMap<Order, OrderRequestDto>().ReverseMap();
        CreateMap<Order, OrderResponseDto>().ReverseMap();
        CreateMap<Category, CategoryRequestDto>().ReverseMap();
        CreateMap<Category, CategoryResponseDto>().ReverseMap();
        CreateMap<OrderDetail, OrderDetailRequestDto>().ReverseMap();
        CreateMap<OrderDetail, OrderDetailResponseDto>().ReverseMap();
        CreateMap<RegisterRequestDto, Member>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName));
    }
}