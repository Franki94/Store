using System;

namespace Store.Customer.Models.Dto
{
    public class CustomerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PaymentMethod { get; set; }
    }
}
