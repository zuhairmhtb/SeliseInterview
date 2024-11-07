using MessageBroker.Events;

namespace MessageBroker.Interfaces
{
    public interface IRabbitMQPublisher<T>
    {
        Task PublishMessageAsync(MQEvent<T> message, string queueName);
    }
}
