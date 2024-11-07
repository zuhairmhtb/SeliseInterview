using MessageBorker.Configuration;
using MessageBroker.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SeliseOMS.Data;

namespace SeliseOMS
{
    public static class Startup
    {
        public static void ConfigureMQ(WebApplicationBuilder builder)
        {
            var mqConfig = builder.Configuration.GetSection("RabbitMQ");
            var settings = new RabbitMQSetting()
            {
                HostName = builder.Configuration.GetValue<string>("RabbitMQ:HostName"),
                UserName = builder.Configuration.GetValue<string>("RabbitMQ:UserName"),
                Password = builder.Configuration.GetValue<string>("RabbitMQ:Password")
            };
            builder.Services.AddSingleton(settings);
            builder.Services.AddScoped(typeof(IRabbitMQPublisher<>), typeof(RabbitMQPublisher<>));
        }

        public static void CreateDatabaseConnection(WebApplicationBuilder builder)
        {
            /**
             * Creates the database connection using EFCore
            */
            var connectionString = builder.Configuration.GetConnectionString("OrderDB");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        public static void BuildServices(WebApplicationBuilder builder)
        {
            /**
             * Dependency injection for services
            */
            ConfigureMQ(builder);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
