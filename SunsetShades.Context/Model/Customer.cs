using System.ComponentModel.DataAnnotations;

namespace SunsetShades.Context.Model
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }
    }
}
