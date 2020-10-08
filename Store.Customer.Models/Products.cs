using System.Collections.Generic;

namespace Store.Customer.Models
{
    public class Products
    {
        public Products()
        {
            CartItemsNavigation = new HashSet<CartItems>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public ICollection<CartItems> CartItemsNavigation { get; set; }
    }
}
