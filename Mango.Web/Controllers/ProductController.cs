using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new();

            ResponseDto? respone = await productService.GetAllProductAsync();

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


        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? respone = await productService.CreateProductAsync(model);

                if (respone != null && respone.IsSucess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = respone?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto? respone = await productService.GetProductByIdAsync(productId);


            if (respone != null && respone.IsSucess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(respone.Result));

                return View(model);
            }
            else
            {
                TempData["error"] = respone?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            ResponseDto? respone = await productService.UpdateProductAsync(productDto);


            if (respone != null && respone.IsSucess)
            {
                TempData["success"] = "Product updated successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = respone?.Message;
            }

            return View(productDto);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? respone = await productService.GetProductByIdAsync(productId);


            if (respone != null && respone.IsSucess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(respone.Result));

                return View(model);
            }
            else
            {
                TempData["error"] = respone?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            ResponseDto? respone = await productService.DeleteProductAsync(productDto.ProductId);


            if (respone != null && respone.IsSucess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = respone?.Message;
            }

            return View(productDto);
        }
    }
}
