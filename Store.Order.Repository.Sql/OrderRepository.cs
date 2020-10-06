namespace Store.Order.Repository.Sql
{
    public class OrderRepository : RepositoryBase<Models.Orders>, IOrderRepository
    {
        public OrderRepository(OrdersDbContext context) : base(context)
        {
        }

    }
}
