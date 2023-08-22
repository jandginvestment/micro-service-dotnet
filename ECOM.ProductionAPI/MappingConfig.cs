using AutoMapper;
using ECOM.Services.ProductAPI.Models;
using ECOM.Services.ProductAPI.Models.DTO;

namespace ECOM.Services.ProductAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ProductDTO, Product>();
            config.CreateMap<Product, ProductDTO>();

        }); return mappingConfig;
    }
}
