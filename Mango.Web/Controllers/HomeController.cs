using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using IdentityModel;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto>? list = new();

            ResponseDto? respone = await _productService.GetAllProductAsync();

            if (respone != null && respone.IsSucess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(respone.Result));
            }
            else
            {
                TempData["error"] = respone?.Message;
            }

            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto? model = new();

            ResponseDto? respone = await _productService.GetProductByIdAsync(productId);

            if (respone != null && respone.IsSucess)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(respone.Result));
            }
            else
            {
                TempData["error"] = respone?.Message;
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()
                }
            };

            ResponseDto? respone = await _productService.GetProductByIdAsync(111);

            if (respone != null && respone.IsSucess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(respone.Result));
            }
            else
            {
                TempData["error"] = respone?.Message;
            }

            return View(new ProductDto());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
