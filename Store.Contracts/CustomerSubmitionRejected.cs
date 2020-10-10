namespace Store.Contracts
{
    public interface CustomerSubmitionRejected
    {
        public string CustomerFirstName { get; }
        public string Reason { get; }
    }
}
