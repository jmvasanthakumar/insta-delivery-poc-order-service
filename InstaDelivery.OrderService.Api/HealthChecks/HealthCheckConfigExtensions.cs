using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InstaDelivery.OrderService.Api.HealthChecks;

public static class HealthCheckConfigExtensions
{
    public static IServiceCollection ConfigureHealthCheck(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddHealthChecks()
                .AddCheck<TestHealthCheck>(
                    name: "test_health_check",
                    tags: ["test"])

                //.AddCheck<DbHealthCheck>(
                //    name: "OrderServiceDb",
                //    tags: ["database"])

                .AddSqlServer(
                    configuration.GetConnectionString("OrderDb")!,
                    name: "OrderServiceDb",
                    tags: ["database"],
                    failureStatus: HealthStatus.Unhealthy)

                .AddCheck("self", () =>
                    HealthCheckResult.Healthy("API is running"), tags: ["api"])

                .AddAzureServiceBusQueue(
                    connectionString: configuration.GetConnectionString("OrderServiceBus"),
                    queueName: "order-events",
                    name: "order-events",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: ["servicebus"]
                );

        services.AddHealthChecksUI(options =>
        {
            options.AddHealthCheckEndpoint("API Health", "/health");
        }).AddInMemoryStorage();

        return services;
    }

    public static WebApplication AddHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecksUI(options =>
        {
            options.UIPath = "/health-ui";
        });

        return app;
    }
}
