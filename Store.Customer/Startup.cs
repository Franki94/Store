using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Store.Customer.Application.Consumers;
using Store.Customer.Application.Contracts;
using Store.Customer.Repository;
using Store.Customer.Repository.Sql;
using Store.Messaging;
using System;

namespace Store.Customer
{
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
            services.AddDbContext<CustomersDbContext>(db => db.UseSqlServer(Configuration.GetConnectionString("CustomersDb")));
            services.AddTransient<ICustomersRepository, CustomersRepository>();

            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                //The consumer will not run here
                //config.AddConsumer<SubmitCustomerConsumer>();
                //config.AddMediator();
                config.AddBus(registrationContext => Bus.Factory.CreateUsingRabbitMq());
                config.AddRequestClient<SubmitCustomer>();
            });

            services.AddMassTransitHostedService();


            services.AddOpenApiDocument(config => config.PostProcess = d => d.Info.Title = "Customers APi");
            //services.AddRabbit();
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
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
