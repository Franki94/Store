using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Store.Customer.Application.Contracts;
using Store.Customer.Models.Dto;
using System.Threading.Tasks;

namespace Store.Customer.Controllers
{
    [Route("customers")]
    public class CustomersController : Controller
    {
        readonly ILogger<CustomersController> _logger;
        readonly IRequestClient<SubmitCustomer> _submitCustomerClient;
        readonly ISendEndpointProvider _sendEndpointProvider;
        public CustomersController(ILogger<CustomersController> logger, IRequestClient<SubmitCustomer> submitCustomerClient,
            ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _submitCustomerClient = submitCustomerClient;
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCustomer([FromBody] CustomerRequest customerRequest)
        {
            var (accepted, rejected) = await _submitCustomerClient.GetResponse<CustomerSubmitionAccepted, CustomerSubmitionRejected>(customerRequest);

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Ok(response);
            }
            var rejectedResponse = await rejected;
            return BadRequest(rejectedResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSubmitCustomer([FromBody] CustomerRequest customerRequest)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new System.Uri("exchange:submit-customer"));

            await endpoint.Send<SubmitCustomer>(customerRequest);
            return Ok();
        }
    }
}
