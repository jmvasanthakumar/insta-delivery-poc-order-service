using Azure.Messaging.ServiceBus;
using InstaDelivery.OrderService.Messaging.Domain;
using InstaDelivery.OrderService.Messaging.Handlers;
using InstaDelivery.OrderService.Messaging.Handlers.Contracts;
using InstaDelivery.OrderService.Messaging.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstaDelivery.OrderService.Messaging;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection ConfigureMessagingServices(this IServiceCollection services)
    {
        services.AddKeyedSingleton<ServiceBusClient>("OrderServiceBus", (sp, key) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("OrderServiceBus");
            return new ServiceBusClient(connectionString);
        });

        services.AddSingleton<IMessageHandler<OrderStatusChange>, OrderStatusChangeHandler>();

        services.AddHostedService<OrderStatusChangeWorker>();

        return services;
    }
}