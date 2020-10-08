using System;
using System.Collections.Generic;

namespace Store.Customer.Models
{
    public class Carts
    {
        public Carts()
        {
            CartItemsNavigation = new HashSet<CartItems>();
        }

        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<CartItems> CartItemsNavigation { get; set; }
        public Customers CustomerNavigation { get; set; }
    }
}
