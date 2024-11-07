namespace MessageBroker.Events
{
    public class MQEvent<T>
    {
        public string EventName { get; set; } = null!;
        public T? Message { get; set; }
    }
}
