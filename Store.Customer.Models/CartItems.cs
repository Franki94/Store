namespace Store.Customer.Models
{
    public class CartItems
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }       
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int CartId { get; set; }

        public Carts CartNavigation { get; set; }
        public Products ProductNavigation { get; set; }
    }
}
