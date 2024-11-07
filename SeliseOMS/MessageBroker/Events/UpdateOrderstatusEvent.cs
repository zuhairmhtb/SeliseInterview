namespace MessageBroker.Events
{
    public class UpdateOrderstatusEvent
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
    }
}
