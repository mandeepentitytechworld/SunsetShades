using SunsetShades.Context.ViewModel;

namespace SunsetShades.Service.Services.Interface
{
    public interface IAccountService
    {
        Task<SignUpViewModel> SignUp(SignUpViewModel model);

        Task<SignUpViewModel> SignIn(SignUpViewModel model);
    }
}
