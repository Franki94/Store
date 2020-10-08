using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Store.Messaging;
using Store.Order.Application.Commands;
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
            services.AddControllers();
            services.AddRabbit();
            services.AddAutoMapper(Assembly.Load("Store.Order.Models.Dto"));

            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddMediatR(typeof(CreateOrderHandler).Assembly);


            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((c, a) =>
                {
                    a.Host("rabbitmq://localhost");
                });
            });

            services.AddMassTransitHostedService();

            //var assembly = Assembly.GetExecutingAssembly();
            //var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(i => i.Name.EndsWith("Mapper")));

            //foreach (var type in types)
            //{
            //    Type abstraction = type.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(i => i.Name.Contains(type.Name));
            //    services.AddSingleton(abstraction, type);
            //}

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
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
