using AutoMapper;
using ECOM.Services.ShoppingCartAPI.Models;
using ECOM.Services.ShoppingCartAPI.Models.DTO;

namespace ECOM.Services.ShoppingCartAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CartHeader, CartHeaderDTO>();
            config.CreateMap<CartHeaderDTO, CartHeader>();

            config.CreateMap<CartDetail, CartDetailDTO>();
            config.CreateMap<CartDetailDTO, CartDetail>();

        }); return mappingConfig;
    }
}
