using Microsoft.Extensions.DependencyInjection;

namespace Store.Messaging
{
    public static class Extensio
    {
        public static IServiceCollection AddRabbit(this IServiceCollection services)
        {
            services.AddSingleton<PublisherQueue>();
            services.AddSingleton<SubscriberQueue>();
            return services;
        }
    }
}
