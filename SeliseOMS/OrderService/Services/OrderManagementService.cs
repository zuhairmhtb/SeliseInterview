using MessageBorker.Configuration;
using MessageBroker.Configuration;
using MessageBroker.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderService.Data;

namespace OrderService.Services
{
    public interface IOrderManagementService
    {

        Task<bool> ProcessMessage(string message);
    }
    public class OrderManagementService : IOrderManagementService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Consumer> _logger;
        private readonly ApplicationDbContext _context;

        public OrderManagementService(
            IServiceProvider serviceProvider,
            ILogger<Consumer> logger,
            ApplicationDbContext context
        )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _context = context;
        }
        async Task<bool> placeOrder(string message)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var orderValidation = JsonConvert.DeserializeObject<PlaceOrderEvent>(message);
                    if (orderValidation == null) return true;

                    Console.Write($"Placed order: {orderValidation.OrderId}");
                    await _context.Orders.AddAsync(new Models.Order() { 
                        Total = orderValidation.Total,
                        CreatedAt = DateTime.Now,
                        Status = "Pending" // move it to enum in a shared class library
                    });

                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
                return false;
            }
        }

        async Task<bool> updateStatus(string message)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var orderStatus = JsonConvert.DeserializeObject<UpdateOrderstatusEvent>(message);
                    if (orderStatus == null) return true;

                    Console.Write($"Updated status: {orderStatus.OrderId}");

                    var existingOrder = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderStatus.OrderId);
                    if (existingOrder == null) return true; // Add proper logging and error handling
                    existingOrder.Status = orderStatus.OrderStatus;
                    _context.Orders.Update(existingOrder);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ProcessMessage(string message)
        {
            var mqMessage = JsonConvert.DeserializeObject<MQEvent<object>>(message);
            Console.WriteLine($"Event name: {mqMessage.EventName}");

            // Ideally this check should be dynamic using Reflections. However, due to 
            // shortage of time I am keeping it hardcoded!!!
            

            bool processedSuccessfully = false;
            try
            {
                switch (mqMessage.EventName)
                {
                    case RabbitMQEvents.PlaceOrder:
                        processedSuccessfully = await placeOrder(message);
                        break;

                    case RabbitMQEvents.UpdateOrderStatus:
                        processedSuccessfully = await updateStatus(message);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while processing message from queue {RabbitMQQueues.OrderQueue}: {ex}");
            }
            return processedSuccessfully;
        }
    }
}
