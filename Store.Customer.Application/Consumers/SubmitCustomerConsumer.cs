using MassTransit;
using Store.Customer.Application.Contracts;
using Store.Customer.Repository;
using Store.Customer.Models;
using System.Threading.Tasks;

namespace Store.Customer.Application.Consumers
{
    public class SubmitCustomerConsumer : IConsumer<SubmitCustomer>
    {
        private readonly ICustomersRepository _customerRepository;
        public SubmitCustomerConsumer(ICustomersRepository customersRepository)
        {
            _customerRepository = customersRepository;
        }
        public async Task Consume(ConsumeContext<SubmitCustomer> context)
        {
            var customer = new Customers { FirstName = context.Message.FirstName, Address = context.Message.Address, LastName = context.Message.LastName, Phone = context.Message.Phone, PaymentMethod = context.Message.PaymentMethod };

            if (customer.FirstName.Contains("Lucas"))
            {
                await context.RespondAsync<CustomerSubmitionRejected>(new { CustomerFirstName = "Lucas", Reason = "it's name is Lucas" });
                return;
            }

            await _customerRepository.Insert(customer);
            await context.RespondAsync<CustomerSubmitionAccepted>(new {CustomerId = customer.CustomerId, CustomerFirstName = customer.FirstName });
        }
    }
}
