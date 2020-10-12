using AutoMapper;
using MassTransit;
using MassTransit.Definition;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Store.Messaging;
using Store.Order.Application.Commands;
using Store.Order.Application.Consumers;
using Store.Order.Repository;
using Store.Order.Repository.Sql;
using System.Reflection;

namespace Store.Order
{
    //public class Person { }
    //public class School { }

    //public class Book { }

    //public interface IBookMapper
    //{
    //    Book MapBookFrom(Person person, School school);
    //}

    //public class BookMapper : IBookMapper
    //{
    //    public Book MapBookFrom(Person person, School school)
    //    {
    //        return new Book();
    //    }
    //}
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OrdersDbContext>(db => db.UseSqlServer(Configuration.GetConnectionString("OrdersDb")));
            services.AddAutoMapper(Assembly.Load("Store.Order.Models.Dto"));

            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ICustomersRepository, CustomersRepository>();
            services.AddMediatR(typeof(CreateOrderHandler).Assembly);

            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(busConfig =>
            {                
                busConfig.AddBus(registrationContext => Bus.Factory.CreateUsingAzureServiceBus(config =>
                {
                    config.Host("Endpoint=sb://notifications-dt.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZTw5lsXeSOCd4ZPreO0L/q/LbpVceqQ0aYgORV2DPqI=");
                    config.ReceiveEndpoint("customer-created", e =>
                    {
                        e.Consumer<CustomerCreatedConsumer>(registrationContext);
                        e.RemoveSubscriptions = true;
                    });                    
                }));                
                busConfig.AddConsumer<CustomerCreatedConsumer>();
            });
            
            services.AddMassTransitHostedService();
            services.AddOpenApiDocument(config => config.PostProcess = d => d.Info.Title = "Orders APi");

            //var assembly = Assembly.GetExecutingAssembly();
            //var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(i => i.Name.EndsWith("Mapper")));

            //foreach (var type in types)
            //{
            //    Type abstraction = type.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(i => i.Name.Contains(type.Name));
            //    services.AddSingleton(abstraction, type);
            //}
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
