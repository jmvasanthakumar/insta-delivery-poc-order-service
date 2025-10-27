using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace InstaDelivery.OrderService.Api.HealthChecks;

public class DbHealthCheck(IConfiguration configuration) : IHealthCheck
{
    private readonly string _connectionString = configuration.GetConnectionString("OrderDb");

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var connStr = _connectionString;

        try
        {
            await using var connection = new SqlConnection(connStr);
            await connection.OpenAsync(cancellationToken);

            await using var cmd = new SqlCommand("SELECT 1", connection);
            var _ = await cmd.ExecuteScalarAsync(cancellationToken);

            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > 2000)
                return HealthCheckResult.Degraded($"DB slow: {stopwatch.ElapsedMilliseconds} ms");

            return HealthCheckResult.Healthy($"DB OK: {stopwatch.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"DB unreachable: {ex.Message}");
        }
    }
}