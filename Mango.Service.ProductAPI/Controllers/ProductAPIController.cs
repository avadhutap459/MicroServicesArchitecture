using AutoMapper;
using Mango.Service.ProductAPI.Data;
using Mango.Service.ProductAPI.Model.Dto;
using Mango.Service.ProductAPI.Models;
using Mango.Service.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private ResponseDto _response;
        private IMapper mapper;

        public ProductAPIController(AppDbContext _appDbContext, IMapper _mapper)
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
                IEnumerable<Product> objList = appDbContext.Products.ToList();
                _response.Result = mapper.Map<IEnumerable<ProductDto>>(objList);

            }
            catch (Exception ex)
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
                Product objList = appDbContext.Products.FirstOrDefault(x => x.ProductId == id);

                _response.Result = mapper.Map<ProductDto>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        [HttpGet("GetByProductName/{productName}")]
        public ResponseDto Get(string productName)
        {
            try
            {
                Product obj = appDbContext.Products.FirstOrDefault(x => x.Name.ToLower() == productName.ToLower());

                if (obj == null)
                {
                    _response.IsSucess = false;
                }

                _response.Result = mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {
                Product obj = mapper.Map<Product>(productDto);

                // appDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Coupons] OFF");
                appDbContext.Products.Add(new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Description = productDto.Description,
                    CategoryName = productDto.CategoryName,
                    ImageUrl = productDto.ImageUrl
                });
                appDbContext.SaveChanges();



                //appDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Coupons] ON");
                _response.Result = mapper.Map<ProductDto>(obj);
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
        public ResponseDto Put([FromBody] ProductDto productDto)
        {
            try
            {
                Product obj = mapper.Map<Product>(productDto);
                appDbContext.Products.Update(obj);
                appDbContext.SaveChanges();


                _response.Result = mapper.Map<ProductDto>(obj);
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
                Product obj = appDbContext.Products.FirstOrDefault(x => x.ProductId == id);

                appDbContext.Products.Remove(obj);
                appDbContext.SaveChanges();


                _response.Result = mapper.Map<ProductDto>(obj);
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
