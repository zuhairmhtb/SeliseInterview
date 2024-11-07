using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Configuration
{
    public class RabbitMQEvents
    {
        // These are all temporary. Ideally it should be detected automatically from type using
        // System.Reflections. Due to time shortage keeping it hardcoded!!!
        public const string PlaceOrder = "PlaceOrderEvent";
        public const string UpdateOrderStatus = "UpdateOrderStatus";

        public const string ReduceProductQuantity = "ReduceProductQuantity";


    }
}
