namespace SunsetShades.Service.Services.Interface
{
    public interface ICartService
    {
        Task<bool> AddToCart(int productId, int userId);
    }
}
