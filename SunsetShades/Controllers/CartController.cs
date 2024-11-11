using Microsoft.AspNetCore.Mvc;
using SunsetShades.Service.Services.Interface;

namespace Sunset_Shades.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var userId = Convert.ToInt32(Request.Cookies["userId"]);
            await cartService.AddToCart(id, userId);
            return this.Ok();
        }
    }
}
