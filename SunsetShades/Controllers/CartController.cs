using Microsoft.AspNetCore.Mvc;
using SunsetShades.Context.Model;
using SunsetShades.Context.ViewModel;
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

        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt32(Request.Cookies["userId"]);
            var result = await cartService.GetCart(userId);
            return View(result);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var userId = Convert.ToInt32(Request.Cookies["userId"]);
            await cartService.AddToCart(id, userId);
            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int cartId)
        {
            var result = await cartService.Remove(cartId);
            return this.Ok();
        }

        public async Task<IActionResult> UpdateCart(int cartId, bool isMinus)
        {
            var result = await cartService.UpdateCart(cartId, isMinus);
            return this.Ok();
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = Convert.ToInt32(Request.Cookies["userId"]);
            var result = await cartService.GetATotalAmt(userId);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel mdoel)
        {
            var userId = Convert.ToInt32(Request.Cookies["userId"]);
            var result = await cartService.Checkout(mdoel, userId);
            return RedirectToAction("MyOrders");
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = Convert.ToInt32(Request.Cookies["userId"]);
            var result = await cartService.GetCart(userId, 1);
            return View(result);
        }
    }
}
