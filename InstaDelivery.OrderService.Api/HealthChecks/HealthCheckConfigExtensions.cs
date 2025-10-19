using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace InstaDelivery.OrderService.Api.HealthChecks;

public static class HealthCheckConfigExtensions
{
    public static IServiceCollection ConfigureHealthCheck(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddHealthChecks()
                .AddSqlServer(
                    configuration.GetConnectionString("OrderDb")!,
                    name: "SQL Database",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy)

                .AddCheck("self", () =>
                    Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API is running"));

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
            options.UIPath = "/health-ui"; // Dashboard path
        });

        return app;
    }
}
