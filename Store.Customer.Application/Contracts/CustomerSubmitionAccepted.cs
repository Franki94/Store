namespace Store.Customer.Application.Contracts
{
    public interface CustomerSubmitionAccepted
    {
        public int CustomerId { get; }
        public string CustomerFirstName { get; }
    }
}
