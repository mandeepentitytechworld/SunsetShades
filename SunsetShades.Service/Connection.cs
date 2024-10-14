using Microsoft.EntityFrameworkCore;
using SunsetShades.Context;

namespace SunsetShades.Service
{
    public static class Connection
    {

        public static bool Open()
        {
            try
            {
                using (var context = new EFDbContext(new DbContextOptions<EFDbContext>()))
                {
                    var brand = context.Brands.FirstOrDefault();
                    if (brand != null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
