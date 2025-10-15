using Microsoft.Extensions.DependencyInjection;

namespace InstaDelivery.OrderService.Application;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<Services.Contracts.IOrderService, Services.OrderService>();

        return services;
    }
}
