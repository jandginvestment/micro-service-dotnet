using AutoMapper;
using ECOM.Services.CouponAPI.Models;
using ECOM.Services.CouponAPI.Models.DTO;

namespace ECOM.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDTO, Coupon>();
                config.CreateMap<Coupon, CouponDTO>();

            }); return mappingConfig;
        }
    }
}
