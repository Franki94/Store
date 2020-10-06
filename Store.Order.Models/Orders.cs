using System.Collections.Generic;

namespace Store.Order.Models
{
    public class Orders
    {
        public Orders()
        {
            OrderItemsNavigation = new HashSet<OrderItems>();
        }
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public int StatusId { get; set; }
        public int CustomerId { get; set; }
        public ICollection<OrderItems> OrderItemsNavigation { get; set; }
        public OrderStatuses OrderStatusNavigation { get; set; }
        public Customers CustomerNavigation { get; set; }
    }
}
