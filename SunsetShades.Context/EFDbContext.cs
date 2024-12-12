using Microsoft.EntityFrameworkCore;
using SunsetShades.Context.Model;

namespace SunsetShades.Context
{
    public class EFDbContext : DbContext
    {
        const string connectionString = "Server=127.0.0.1; User ID=root; Password=; Database=sunsetdb";
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
