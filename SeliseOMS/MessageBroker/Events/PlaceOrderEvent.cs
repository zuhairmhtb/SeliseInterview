namespace MessageBroker.Events
{
    public class PlaceOrderEvent
    {
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public List<int> Products { get; set; } = new List<int>();
    }
}
