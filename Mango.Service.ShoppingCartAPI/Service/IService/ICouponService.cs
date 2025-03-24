using Mango.Service.ShoppingCartAPI.Model.Dto;

namespace Mango.Service.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
