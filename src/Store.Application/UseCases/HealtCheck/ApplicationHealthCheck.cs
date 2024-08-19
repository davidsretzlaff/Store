using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Store.Application.UseCases.HealtCheck
{
	public class ApplicationHealthCheck : IHealthCheck
	{
		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			return Task.FromResult(HealthCheckResult.Healthy("Application is healthy."));
		}
	}
}
