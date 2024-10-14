namespace SunsetShades.Context.ViewModel
{
    public class ProductsViewModel
    {
        public List<BrandViewModel> Brands { get; set; }

        public List<CategoryViewModel> Categories { get; set; }

        public List<ProductViewModel> Products { get; set; }

        public string Category {  get; set; }

        public string Brand {  get; set; }

        public string Size {  get; set; }

        public ResponseMessage ResponseMessage { get; set; }
    }

    public class BrandViewModel
    {
        public int BrandId { get; set; }

        public string BrandName { get; set; }
    }

    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }

    public class ProductViewModel : BrandViewModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string Size { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string FrindlyName { get; set; }

        public string FrameColor { get; set; }

        public string GlassColor { get; set; }

        public string Material {  get; set; }

        public CategoryViewModel Category { get; set; }
    }
}
