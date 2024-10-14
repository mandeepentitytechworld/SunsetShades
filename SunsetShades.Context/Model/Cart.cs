namespace SunsetShades.Context.Model
{
    public class Cart
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual Product Product { get; set; }

        public virtual Customers Customer { get; set; }
    }
}
