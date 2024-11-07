namespace MessageBroker.Events
{
    public class AddProductEvent
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
    }
}
