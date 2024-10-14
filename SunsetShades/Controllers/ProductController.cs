using Microsoft.AspNetCore.Mvc;
using SunsetShades.Service.Services.Interface;

namespace Sunset_Shades.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await productService.GetTop20Products();
            return View(result);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var result = await productService.GetProductById(id);
            return View(result);
        }

        public async Task<IActionResult> Brand(string id)
        {
            var result = await productService.GetTop20ProductsByBrandId(id);
            return PartialView("Views/Product/_Index.cshtml", result);
        }

        public async Task<IActionResult> Category(string id)
        {
            var result = await productService.GetTop20ProductsByCategoryId(category: id);
            return PartialView("Views/Product/_Index.cshtml", result);
        }
    }
}
