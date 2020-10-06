namespace Store.Order.Application.Commands
{
    public class CreatedOrder
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
    }
}
