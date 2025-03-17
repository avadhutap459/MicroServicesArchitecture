using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService couponService;
        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();

            ResponseDto? respone = await couponService.GetAllCouponAsync();

            if(respone!= null && respone.IsSucess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(respone.Result));
            }
            else
            {
                TempData["error"] = respone?.Message;
            }

            return View(list);
        }


        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if(ModelState.IsValid)
            {
                ResponseDto? respone = await couponService.CreateCouponAsync(model);

                if (respone != null && respone.IsSucess)
                {
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = respone?.Message;
                }
            }
            return View(model);
        }


        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDto? respone = await couponService.GetCouponByIdAsync(couponId);


            if (respone != null && respone.IsSucess)
            {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(respone.Result));

                return View(model);
            }
            else
            {
                TempData["error"] = respone?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            ResponseDto? respone = await couponService.DeleteCouponAsync(couponDto.CouponId);


            if (respone != null && respone.IsSucess)
            {
                TempData["success"] = "Coupon deleted successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = respone?.Message;
            }

            return View(couponDto);
        }
    }
}
