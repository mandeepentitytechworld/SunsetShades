using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunsetShades.Context.Model
{
    [Table("brands")]
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        public string BrandName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
