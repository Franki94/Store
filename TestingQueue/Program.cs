using MassTransit;
using MassTransit.Definition;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Store.Customer.Application.Consumers;
using Store.Customer.Repository;
using Store.Customer.Repository.Sql;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TestingQueue
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", true);
                    config.AddEnvironmentVariables();

                    //if (args != null)
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostedService, services) =>
                {
                    services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
                    services.AddDbContext<CustomersDbContext>(db => db.UseSqlServer(hostedService.Configuration.GetConnectionString("CustomersDb")));
                    services.AddTransient<ICustomersRepository, CustomersRepository>();
                    services.AddMassTransit(config =>
                    {
                        config.AddConsumersFromNamespaceContaining<SubmitCustomerConsumer>();
                        config.AddBus(ConfigureBus);
                    });
                    services.AddHostedService<MassTransitConsoleHostetService>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("logging"));
                    //logging.ConsoleLoggerExtensions();
                    //logging.AddConsole();
                    logging.AddConsole();
                });

            if (isService)
                await builder.UseWindowsService().Build().RunAsync();
            else
                await builder.RunConsoleAsync();

            //var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>

            //{
            //    sbc.Host("rabbitmq://localhost");

            //    sbc.ReceiveEndpoint("test_queue", ep =>
            //    {                    
            //        ep.Handler<Message>(context =>
            //        {
            //            return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
            //        });
            //    });
            //});

            //await bus.StartAsync(); // This is important!

            //await bus.Publish(new Message { Text = "Hi" });
            //await bus.Publish(new Message { Text = "Becker" });

            //Console.WriteLine("Press any key to exit");
            //await Task.Run(() => Console.ReadKey());

            //await bus.StopAsync();
        }

        public static IBusControl ConfigureBus(IBusRegistrationContext provider)
        {
            //Bus.Factory.CreateUsingRabbitMq(config =>
            //{
            //    config.Host("rabbitmq://localhost", "VirtualHost",
            //        hostConfigurator =>
            //        {
            //            hostConfigurator.Username("guest");
            //            hostConfigurator.Password("guest");
            //        });
            //});                
            return Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.ConfigureEndpoints(provider);
            });
        }
    }
}