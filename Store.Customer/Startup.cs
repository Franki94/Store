using MassTransit;
using MassTransit.Definition;
using MassTransit.MultiBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Store.Contracts;
using Store.Customer.Repository;
using Store.Customer.Repository.Sql;

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
                //config.AddBus(registrationContext => Bus.Factory.CreateUsingRabbitMq());

                var azureServiceBus = Bus.Factory.CreateUsingAzureServiceBus(busFactoryConfig =>
                {
                    // specify the message FlightOrder to be sent to a specific topic
                    //busFactoryConfig.Message<FlightOrder>(configTopology =>
                    //{
                    //    configTopology.SetEntityName(flightOrdersTopic);
                    //});
                    
                    busFactoryConfig.SelectBasicTier();
                    busFactoryConfig.Host("connection");
                    //busFactoryConfig.Message<SubmitCustomer>(m => 
                    //{
                    //    m.SetEntityName("submit-customer");
                    //});
                });
                services.AddSingleton<IPublishEndpoint>(azureServiceBus);
                services.AddSingleton<ISendEndpointProvider>(azureServiceBus);
                services.AddSingleton<IBus>(azureServiceBus);
                config.AddBus(registrationContext => azureServiceBus);            

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
