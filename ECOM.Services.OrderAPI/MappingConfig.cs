using AutoMapper;
using ECOM.Services.OrderAPI.Models;
using ECOM.Services.OrderAPI.Models.DTO;

namespace ECOM.Services.ShoppingOrderAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<OrderHeaderDTO, CartHeaderDTO>()
            .ForMember(d => d.CartTotal, u => u.MapFrom(s => s.OrderTotal)).ReverseMap();


            config.CreateMap<CartDetailDTO, OrderDetailDTO>()
            .ForMember(d => d.ProductName, u => u.MapFrom(s => s.Product.Name))
            .ForMember(d => d.Price, u => u.MapFrom(s => s.Product.Price));

            config.CreateMap<OrderDetailDTO, CartDetailDTO>();

            config.CreateMap<OrderHeader, OrderHeaderDTO>().ReverseMap();
            config.CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();



        }); return mappingConfig;
    }
}
