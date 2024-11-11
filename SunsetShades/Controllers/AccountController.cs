using Microsoft.AspNetCore.Mvc;
using SunsetShades.Context.ViewModel;
using SunsetShades.Service.Services.Interface;

namespace Sunset_Shades.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public IActionResult Index()
        {
            if (Convert.ToInt32(Request.Cookies["userId"]) > 0)
            {
                return View();
            }
            return View("Error.cshtml");
        }

        public IActionResult SignUp()
        {
            var model = new SignUpViewModel();
            model.ResponseMessage = new ResponseMessage();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            var result = await accountService.SignUp(model);

            if (result.ResponseMessage.IsValid)
            {
                Response.Cookies.Append("userId", result.Id.ToString());
                Response.Cookies.Append("userName", result.Name.ToString());
                Response.Cookies.Append("userEmail", result.Email.ToString());

                return RedirectToAction("Index", "Cart");
            }

            return View(result);
        }

        public IActionResult SignIn()
        {
            var result = new SignUpViewModel();
            result.ResponseMessage = new ResponseMessage();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignUpViewModel model)
        {
            var result = await accountService.SignIn(model);

            if (result.ResponseMessage.IsValid)
            {
                Response.Cookies.Append("userId", result.Id.ToString());
                Response.Cookies.Append("userName", result.Name.ToString());
                Response.Cookies.Append("userEmail", result.Email.ToString());

                return RedirectToAction("Index", "Cart");
            }

            return View(result);
        }

        public IActionResult SignOut()
        {
            Response.Cookies.Delete("userId");
            Response.Cookies.Delete("userName");
            Response.Cookies.Delete("userEmail");

            return RedirectToAction("SignIn", "Account");
        }

    }
}
