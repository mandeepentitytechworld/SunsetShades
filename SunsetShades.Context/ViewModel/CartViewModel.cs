namespace SunsetShades.Context.ViewModel
{
    public class CartViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public ResponseMessage ResponseMessage { get; set; }

        public decimal Total { get; set; }
    }
}
