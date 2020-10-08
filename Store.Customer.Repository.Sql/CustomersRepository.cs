using Store.Customer.Models;

namespace Store.Customer.Repository.Sql
{
    public class CustomersRepository : RepositoryBase<Customers>, ICustomersRepository
    {
        public CustomersRepository(CustomersDbContext context) : base(context)
        {
        }
    }
}
