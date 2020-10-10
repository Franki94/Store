namespace Store.Contracts
{
    public interface SubmitCustomer
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Address { get; }
        public string Phone { get; }
        public string PaymentMethod { get; }
    }
}
