using InstaDelivery.OrderService.Repository.Context;
using InstaDelivery.OrderService.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstaDelivery.OrderService.Repository;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OrderDb");

        services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
