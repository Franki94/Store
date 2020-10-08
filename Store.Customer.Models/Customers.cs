namespace Store.Customer.Models
{
    public class Customers
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PaymentMethod { get; set; }

        public Carts CarNavigation { get; set; }
    }
}
