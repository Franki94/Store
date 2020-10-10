using Store.Order.Models;

namespace Store.Order.Repository.Sql
{
    public class CustomersRepository : RepositoryBase<Customers>, ICustomersRepository
    {
        public CustomersRepository(OrdersDbContext context) : base(context)
        {
        }
    }
}
