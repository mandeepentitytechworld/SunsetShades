using Microsoft.EntityFrameworkCore;
using SunsetShades.Context;
using SunsetShades.Context.Model;
using SunsetShades.Service.Services.Interface;
using System.Net.Http;

namespace SunsetShades.Service.Services
{
    public class CartService : ICartService
    {
        private readonly EFDbContext eFDbContext;

        public CartService(EFDbContext eFDbContext)
        {
            this.eFDbContext = eFDbContext;
        }


        public async Task<bool> AddToCart(int productId, int userId)
        {
            try
            {
                var dbCart = await eFDbContext.Carts.Where(c => c.CustomerId == userId && c.ProductId == productId).Select(c => c).FirstOrDefaultAsync();
                if (dbCart != null)
                {
                    dbCart.Quantity++;
                    dbCart.UpdatedAt = DateTime.UtcNow;

                    eFDbContext.Carts.Update(dbCart);
                    await eFDbContext.SaveChangesAsync();
                }
                else
                {
                    var cart = new Cart
                    {
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,

                        CustomerId = userId,
                        ProductId = productId,
                        Quantity = 1,
                    };

                    await eFDbContext.Carts.AddAsync(cart);
                    await eFDbContext.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
