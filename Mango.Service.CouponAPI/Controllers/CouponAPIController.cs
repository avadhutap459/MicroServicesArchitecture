using AutoMapper;
using Mango.Service.CouponAPI.Data;
using Mango.Service.CouponAPI.Model;
using Mango.Service.CouponAPI.Model.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private ResponseDto _response;
        private IMapper mapper;

        public CouponAPIController(AppDbContext _appDbContext,IMapper _mapper)
        {
            appDbContext = _appDbContext;
            _response = new ResponseDto();
            mapper = _mapper;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> objList = appDbContext.Coupons.ToList();
                _response.Result = mapper.Map<IEnumerable<CouponDto>>(objList);

            }
            catch(Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon objList = appDbContext.Coupons.FirstOrDefault(x=>x.CouponId == id);

                _response.Result = mapper.Map<CouponDto>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        [HttpGet("GetByCouponCode/{couponCode}")]
        public ResponseDto Get(string couponCode)
        {
            try
            {
                Coupon obj = appDbContext.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == couponCode.ToLower());

                if(obj == null)
                {
                    _response.IsSucess =false;
                }

                _response.Result = mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = mapper.Map<Coupon>(couponDto);

               // appDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Coupons] OFF");
                appDbContext.Coupons.Add(new Coupon
                {
                    CouponCode = couponDto.CouponCode,
                    DiscountAmount = couponDto.DiscountAmount,
                    MinAmount = couponDto.MinAmount,
                });
                appDbContext.SaveChanges();



                //appDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Coupons] ON");
                _response.Result = mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = mapper.Map<Coupon>(couponDto);
                appDbContext.Coupons.Update(obj);
                appDbContext.SaveChanges();


                _response.Result = mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon obj = appDbContext.Coupons.FirstOrDefault(x=>x.CouponId == id);

                appDbContext.Coupons.Remove(obj);
                appDbContext.SaveChanges();


                _response.Result = mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


    }
}
