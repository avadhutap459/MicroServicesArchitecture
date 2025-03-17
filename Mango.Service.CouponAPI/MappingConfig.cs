using AutoMapper;
using Mango.Service.CouponAPI.Model;
using Mango.Service.CouponAPI.Model.Dto;

namespace Mango.Service.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>();
                config.CreateMap<Coupon, CouponDto>();
            });

            return mapperConfig;
        }
    }
}
