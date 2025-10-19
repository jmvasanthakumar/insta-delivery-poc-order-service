using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InstaDelivery.OrderService.Api.HealthChecks
{
    public class TestHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var randomState = new Random().Next(1, 4);
            return randomState switch
            {
                1 => Task.FromResult(HealthCheckResult.Healthy("Test is healthy")),
                2 => Task.FromResult(HealthCheckResult.Degraded("Test is degraded")),
                _ => Task.FromResult(HealthCheckResult.Unhealthy("Test is unhealthy")),
            };
        }
    }
}
