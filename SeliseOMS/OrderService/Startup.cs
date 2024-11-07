using MessageBorker.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.Data;
using OrderService.Services;

namespace OrderService
{
    public static class Startup
    {
        public static void ConfigureRabbitMQ(HostBuilderContext context, IServiceCollection services)
        {
            var mqConfig = context.Configuration.GetSection("RabbitMQ");
            services.Configure<RabbitMQSetting>(mqConfig);
            services.AddHostedService<Consumer>();
        }

        public static void ConfigureDatabase(HostBuilderContext context, IServiceCollection services)
        {
            /**
             * Creates the database connection using EFCore
            */
            var connectionString = context.Configuration.GetConnectionString("OrderDB");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<IOrderManagementService, OrderManagementService>();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                 .ConfigureAppConfiguration((context, config) =>
                 {
                     config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                 })
                .ConfigureServices((context, services) => {
                    services.AddOptions();
                    ConfigureDatabase(context, services);
                    ConfigureRabbitMQ(context, services);
                    ConfigureServices(context, services);

                });
        }
    }
}
