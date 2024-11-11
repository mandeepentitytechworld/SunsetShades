using Microsoft.EntityFrameworkCore;
using SunsetShades.Context;
using SunsetShades.Context.Model;
using SunsetShades.Context.ViewModel;
using SunsetShades.Service.Services.Interface;

namespace SunsetShades.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly EFDbContext eFDbContext;

        public AccountService(EFDbContext eFDbContext)
        {
            this.eFDbContext = eFDbContext;
        }

        public async Task<SignUpViewModel> SignUp(SignUpViewModel model)
        {
            var result = new SignUpViewModel();
            result.ResponseMessage = new ResponseMessage();

            try
            {
                if (Connection.Open())
                {
                    var existingUser = await eFDbContext.Customers.Where(c=>c.Email == model.Email).FirstOrDefaultAsync();
                    if (existingUser != null)
                    {
                        result.ResponseMessage.Message = $"User with email {model.Email} already exist.";
                        result.ResponseMessage.IsValid = false;

                        return result;
                    }

                    var customer = new Customers
                    {
                        Address = model.Address,
                        Email = model.Email,
                        Name = model.Name,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber,
                    };

                    await eFDbContext.Customers.AddAsync(customer);
                    await eFDbContext.SaveChangesAsync();

                    model.Id = customer.Id;
                    result = model;
                    result.ResponseMessage = new ResponseMessage();
                }
                else
                {
                    result.ResponseMessage.Message = "Not conected to database";
                    result.ResponseMessage.IsValid = false;
                }
            }
            catch (Exception)
            {
                result.ResponseMessage.Message = "Internal server error.";
                result.ResponseMessage.IsValid = false;
            }

            return result;
        }

        public async Task<SignUpViewModel> SignIn(SignUpViewModel model)
        {
            var result = new SignUpViewModel();
            result.ResponseMessage = new ResponseMessage();

            try
            {
                if (Connection.Open())
                {
                    var existingUser = await eFDbContext.Customers.Where(c => c.Email == model.Email && c.Password == model.Password).FirstOrDefaultAsync();
                    
                    if (existingUser == null)
                    {
                        result.ResponseMessage.Message = $"Incorrect user details.";
                        result.ResponseMessage.IsValid = false;

                        return result;
                    }

                    result.Id = existingUser.Id;
                    result.Name = existingUser.Name;
                    result.Email = existingUser.Email;
                    
                    result.ResponseMessage = new ResponseMessage();
                }
                else
                {
                    result.ResponseMessage.Message = "Not conected to database";
                    result.ResponseMessage.IsValid = false;
                }
            }
            catch (Exception)
            {
                result.ResponseMessage.Message = "Internal server error.";
                result.ResponseMessage.IsValid = false;
            }

            return result;
        }
    }
}
