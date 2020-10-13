using MassTransit;
using MassTransit.Definition;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Store.Contracts;
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

                    if (args != null)
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
                        config.AddConsumer<SubmitCustomerConsumer>();
                        
                    });
                    services.AddHostedService<MassTransitConsoleHostetService>();                    
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("logging"));                
                    logging.AddConsole();
                });

            if (isService)
                await builder.UseWindowsService().Build().RunAsync();
            else
                await builder.RunConsoleAsync();
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
            //return Bus.Factory.CreateUsingRabbitMq(config =>
            //{
            //    config.ConfigureEndpoints(provider);
            //});

            return Bus.Factory.CreateUsingAzureServiceBus(config =>
            {
                config.Host("connection");
                //config.ConfigureEndpoints(provider);

                //config.Message<SubmitCustomer>(m =>
                //{
                //    m.SetEntityName("submit-customer");
                //});
                config.ReceiveEndpoint("submit-customer", e =>
                {
                    e.Consumer<SubmitCustomerConsumer>(provider);
                });                
            });
        }
    }
}