using Microsoft.AspNetCore.Mvc;
using SunsetShades.Context.ViewModel;

namespace Sunset_Shades.Components
{
    [ViewComponent(Name = "Account")]
    public class AccountComponent: ViewComponent
    {
        public AccountComponent() { }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.Request.Cookies["userId"];
            var userName = HttpContext.Request.Cookies["userName"];

            var userDetail = new UserDetail();
            userDetail.Id = Convert.ToInt32(userId);
            userDetail.Name = Convert.ToString(userName);

            return View("Account", userDetail);
        }
    }
}
