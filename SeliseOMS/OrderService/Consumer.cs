using MessageBorker.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBroker.Events;
using OrderService.Services;

namespace OrderService
{
    public class Consumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Consumer> _logger;
        private readonly IOrderManagementService _orderManagementService;

        private readonly RabbitMQSetting _rabbitMqSetting;
        private IConnection _connection;
        private IModel _channel;

        public Consumer(
            IOptions<RabbitMQSetting> rabbitMqSetting,
            IServiceProvider serviceProvider,
            IOrderManagementService orderManagementService,
            ILogger<Consumer> logger)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;
            _serviceProvider = serviceProvider;
            _orderManagementService = orderManagementService;
            _logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }


        private EventHandler<BasicDeliverEventArgs> GetPlacOrderHandler()
        {
            return async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var processedSuccessfully = await _orderManagementService.ProcessMessage(message);

                if (processedSuccessfully)
                {
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    _channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                }
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            StartConsuming(
                RabbitMQQueues.OrderQueue, 
                stoppingToken, 
                GetPlacOrderHandler()
            );

            await Task.CompletedTask;
        }

        private void StartConsuming(
            string queueName, 
            CancellationToken cancellationToken,
            EventHandler<BasicDeliverEventArgs> handler
        )
        {
            _channel.QueueDeclare(
                queue: queueName, 
                durable: false, 
                exclusive: false, 
                autoDelete: false, 
                arguments: null
            );

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += handler;

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
