using MassTransit;
using Store.Contracts;
using Store.Order.Repository;
using System.Threading.Tasks;

namespace Store.Order.Application.Consumers
{
    public class CustomerCreatedConsumer : IConsumer<CustomerCreated>
    {
        private readonly ICustomersRepository _customersRepository;
        public CustomerCreatedConsumer(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }
        public async Task Consume(ConsumeContext<CustomerCreated> context)
        {
            var customer = context.Message;
            await _customersRepository.Insert(new Models.Customers
            {
                Address = customer.Address,
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone
            });
        }
    }
}
