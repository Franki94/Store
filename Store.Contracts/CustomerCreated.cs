namespace Store.Contracts
{
    public interface CustomerCreated
    {
        public int CustomerId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Address { get; }
        public string Phone { get; }
    }
}
