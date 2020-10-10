using MassTransit;
using Store.Contracts;
using Store.Customer.Models;
using Store.Customer.Repository;
using System.Threading.Tasks;

namespace Store.Customer.Application.Consumers
{
    public class SubmitCustomerConsumer : IConsumer<SubmitCustomer>
    {
        private readonly ICustomersRepository _customerRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        public SubmitCustomerConsumer(ICustomersRepository customersRepository, IPublishEndpoint publish)
        {
            _customerRepository = customersRepository;
            _publishEndpoint = publish;
        }
        public async Task Consume(ConsumeContext<SubmitCustomer> context)
        {
            var customer = new Customers { FirstName = context.Message.FirstName, Address = context.Message.Address, LastName = context.Message.LastName, Phone = context.Message.Phone, PaymentMethod = context.Message.PaymentMethod };

            if (customer.FirstName.Contains("Lucas"))
            {
                if (context.RequestId != null)
                    await context.RespondAsync<CustomerSubmitionRejected>(new { CustomerFirstName = "Lucas", Reason = "it's name is Lucas" });
                return;
            }

            await _customerRepository.Insert(customer);

            await _publishEndpoint.Publish<CustomerCreated>(new
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Phone = customer.Phone,
                CustomerId = customer.CustomerId
            });
            
            //await context.Publish<CustomerCreated>(new
            //{
            //    FirstName = customer.FirstName,
            //    LastName = customer.LastName,
            //    Address = customer.Address,
            //    Phone = customer.Phone,
            //    CustomerId = customer.CustomerId
            //});
            if (context.RequestId != null)
                await context.RespondAsync<CustomerSubmitionAccepted>(new { CustomerId = customer.CustomerId, CustomerFirstName = customer.FirstName });
        }
    }
}
