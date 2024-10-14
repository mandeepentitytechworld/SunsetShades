using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunsetShades.Context.Model
{
    [Table("product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string Size { get; set; }

        public string Image { get; set; }

        public int BrandId { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public string FriendlyName { get; set; }

        public string FrameColor { get; set; }

        public string GlassColor { get; set; }

        public string Material { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Category Category { get; set; }
    }
}
