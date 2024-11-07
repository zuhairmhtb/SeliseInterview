// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using OrderService;


IHost host = Startup.CreateHostBuilder(args).Build();
host.Run();