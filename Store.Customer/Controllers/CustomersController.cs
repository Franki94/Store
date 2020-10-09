﻿using MassTransit;
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
        public CustomersController(ILogger<CustomersController> logger, IRequestClient<SubmitCustomer> submitCustomerClient)
        {
            _logger = logger;
            _submitCustomerClient = submitCustomerClient;
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
    }
}
