using AutoMapper;
using Mango.Service.ShoppingCartAPI.Models;
using Mango.Service.ShoppingCartAPI.Models.Dto;

namespace Mango.Service.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartDetailsDto, CartDetails>();
                config.CreateMap<CartHeaderDto, CartHeader>();
            });

            return mapperConfig;
        }
    }
}
