using Microsoft.EntityFrameworkCore;
using SunsetShades.Context;
using SunsetShades.Context.Model;
using SunsetShades.Context.ViewModel;
using SunsetShades.Service.Services.Interface;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Numerics;

namespace SunsetShades.Service.Services
{
    public class CartService : ICartService
    {
        private readonly EFDbContext eFDbContext;

        public CartService(EFDbContext eFDbContext)
        {
            this.eFDbContext = eFDbContext;
        }


        public async Task<CartViewModel> GetCart(int userId, int placed = 0)
        {
            var result = new CartViewModel();
            result.ResponseMessage = new ResponseMessage();

            try
            {
                var dbCart = new List<Cart>();
                if (placed == 0)
                {
                    dbCart = await eFDbContext.Carts.Where(c => c.CustomerId == userId && (c.OrderId == 0)).Select(c => c).ToListAsync();
                }
                else
                {
                    dbCart = await eFDbContext.Carts.Where(c => c.CustomerId == userId && (c.OrderId > 0)).Select(c => c).ToListAsync();
                }

                result.Products = new List<ProductViewModel>();

                foreach (var cart in dbCart)
                {
                    var dbProduct = await eFDbContext.Product.Where(c => c.Id == cart.ProductId).FirstOrDefaultAsync();
                    var dbProductBrand = await eFDbContext.Brands.Where(c => c.Id == dbProduct.BrandId).FirstOrDefaultAsync();

                    var p = new ProductViewModel()
                    {
                        Id = dbProduct.Id,
                        ProductName = dbProduct.ProductName,
                        Image = dbProduct.Image,
                        BrandName = dbProductBrand.BrandName,
                        Size = dbProduct.Size,
                        Quantity = cart.Quantity,
                        Price = dbProduct.Price,

                        CartId = cart.Id,
                    };


                    if (placed == 1)
                    {
                        var myOrder = await eFDbContext.CustomerOrders.Where(c => c.OrderId == cart.OrderId).FirstOrDefaultAsync();

                        if (myOrder != null)
                        {
                            p.OrderDate = myOrder.CreatedAt;
                        }
                    }

                    result.Products.Add(p);
                }

                decimal total = 0;

                foreach (var cart in dbCart)
                {
                    var p = await eFDbContext.Product.Where(c => c.Id == cart.ProductId).FirstOrDefaultAsync();

                    total = total + (p.Price * cart.Quantity);
                }

                result.Total = total;
            }
            catch (Exception)
            {
                result.ResponseMessage.IsValid = true;
                result.ResponseMessage.Message = "Server Error";
            }

            return result;
        }

        public async Task<bool> AddToCart(int productId, int userId)
        {
            try
            {
                var dbCart = await eFDbContext.Carts.Where(c => c.CustomerId == userId && c.ProductId == productId && c.OrderId == 0).Select(c => c).FirstOrDefaultAsync();
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

        public async Task<bool> Remove(int cartId)
        {
            try
            {
                var dbCart = await eFDbContext.Carts.Where(c => c.Id == cartId).Select(c => c).FirstOrDefaultAsync();

                if (dbCart != null)
                {
                    eFDbContext.Carts.Remove(dbCart);
                    await eFDbContext.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<decimal> GetATotalAmt(int userId)
        {
            var dbCart = await eFDbContext.Carts.Where(c => c.CustomerId == userId && c.OrderId == 0).Select(c => c).ToListAsync();

            decimal total = 0;

            foreach (var cart in dbCart)
            {
                var p = await eFDbContext.Product.Where(c=>c.Id == cart.ProductId).FirstOrDefaultAsync();

                total = total + (p.Price * cart.Quantity);
            }

            return total;
        }

        public async Task<bool> UpdateCart(int cartId, bool isMinus)
        {
            try
            {
                var dbCart = await eFDbContext.Carts.Where(c => c.Id == cartId).Select(c => c).FirstOrDefaultAsync();

                if (dbCart != null)
                {
                    if (isMinus)
                    {
                        dbCart.Quantity--;

                        if (dbCart.Quantity <= 0)
                        {
                            eFDbContext.Carts.Remove(dbCart);
                        }
                        else
                        {
                            eFDbContext.Carts.Update(dbCart);
                        }
                    }
                    else
                    {
                        dbCart.Quantity++;
                        eFDbContext.Carts.Update(dbCart);
                    }
                    await eFDbContext.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Checkout(CheckoutViewModel mdoel, int userId)
        {
            try
            {
                if (mdoel != null)
                {
                    var model = new CustomerOrder
                    {
                        CreatedAt = DateTime.Now,

                        Address = mdoel.Address,
                        CardNumber = mdoel.CardNumber,
                        City = mdoel.City,
                        CVV = mdoel.CVV,
                        Email = mdoel.Email,
                        ExpiryDate = mdoel.ExpiryDate,
                        FullName = mdoel.FullName,
                        State = mdoel.State,
                        ZipCode = mdoel.ZipCode,
                    };

                    await eFDbContext.CustomerOrders.AddAsync(model);
                    await eFDbContext.SaveChangesAsync();

                    var dbCart = await eFDbContext.Carts.Where(c => c.CustomerId == userId && c.OrderId == 0).Select(c => c).ToListAsync();

                    if (dbCart?.Count > 0)
                    {
                        dbCart.ForEach(item => item.OrderId = model.OrderId);

                        eFDbContext.Carts.UpdateRange(dbCart);
                        await eFDbContext.SaveChangesAsync();
                    }

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
