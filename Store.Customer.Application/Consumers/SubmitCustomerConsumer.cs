using MassTransit;
using Microsoft.VisualBasic.CompilerServices;
using Store.Contracts;
using Store.Customer.Models;
using Store.Customer.Repository;
using System;
using System.Threading.Tasks;

namespace Store.Customer.Application.Consumers
{
    public class SubmitCustomerConsumer : IConsumer<SubmitCustomer>
    {
        private readonly ICustomersRepository _customerRepository;
        //private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        public SubmitCustomerConsumer(ICustomersRepository customersRepository, ISendEndpointProvider sendEndpointProvider /* IPublishEndpoint publish*/)
        {
            _customerRepository = customersRepository;
            //publish create a topic, we can't use this in basic tier
            //_publishEndpoint = publish;
            _sendEndpointProvider = sendEndpointProvider;
        }
        public async Task Consume(ConsumeContext<SubmitCustomer> context)
        {
            var customer = new Customers { FirstName = context.Message.FirstName, Address = context.Message.Address, LastName = context.Message.LastName, Phone = context.Message.Phone, PaymentMethod = context.Message.PaymentMethod };

            //if (customer.FirstName.Contains("Lucas"))
            //{
            //    if (context.RequestId != null)
            //        await context.RespondAsync<CustomerSubmitionRejected>(new { CustomerFirstName = "Lucas", Reason = "it's name is Lucas" });
            //    return;
            //}

            await _customerRepository.Insert(customer);

            //publish create a topic, we can't use this in basic tier
            //await _publishEndpoint.Publish<CustomerCreated>(new
            //{
            //    FirstName = customer.FirstName,
            //    LastName = customer.LastName,
            //    Address = customer.Address,
            //    Phone = customer.Phone,
            //    CustomerId = customer.CustomerId
            //});
            await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:customer-created"));
            //await context.Publish<CustomerCreated>(new
            //{
            //    FirstName = customer.FirstName,
            //    LastName = customer.LastName,
            //    Address = customer.Address,
            //    Phone = customer.Phone,
            //    CustomerId = customer.CustomerId
            //});
            //if (context.RequestId != null)
            //    await context.RespondAsync<CustomerSubmitionAccepted>(new { CustomerId = customer.CustomerId, CustomerFirstName = customer.FirstName });
        }
    }
}
