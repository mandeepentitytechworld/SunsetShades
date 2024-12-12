using SunsetShades.Context.ViewModel;

namespace SunsetShades.Service.Services.Interface
{
    public interface ICartService
    {
        Task<CartViewModel> GetCart(int userId, int placed = 0);

        Task<bool> AddToCart(int productId, int userId);

        Task<bool> Remove(int cartId);

        Task<bool> UpdateCart(int cartId, bool isMinus);

        Task<decimal> GetATotalAmt(int userId);

        Task<bool> Checkout(CheckoutViewModel mdoel, int userId);
    }
}
