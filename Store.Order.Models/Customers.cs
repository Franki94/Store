using System.Collections;
using System.Collections.Generic;

namespace Store.Order.Models
{
    public class Customers
    {
        public Customers()
        {
            OrdersNavigation = new HashSet<Models.Orders>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public ICollection<Models.Orders> OrdersNavigation { get; set; }
    }
}
