using System.Collections.Generic;

namespace Store.Order.Models
{
    public class OrderStatuses
    {
        public OrderStatuses()
        {
            OrdersNavigation = new HashSet<Orders>();
        }
        public int OrderStatusId { get; set; }
        public string Description { get; set; }

        public ICollection<Orders> OrdersNavigation { get; set; }
    }
}
