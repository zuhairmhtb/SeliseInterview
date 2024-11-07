namespace MessageBroker.Events
{
    public class ReduceProductInventoryEvent
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
