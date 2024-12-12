namespace SunsetShades.Context.ViewModel
{
    public class CheckoutViewModel
    {
        // Personal Details
        public string FullName { get; set; }

        public string Email { get; set; }

        // Address Details
        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        // Payment Details
        public string CardNumber { get; set; }

        public string ExpiryDate { get; set; }

        public int CVV { get; set; }
    }
}
