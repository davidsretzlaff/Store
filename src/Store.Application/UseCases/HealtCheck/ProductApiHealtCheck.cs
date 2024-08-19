using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Store.Application.UseCases.HealtCheck
{
	public class ProductApiHealtCheck : IHealthCheck
	{
		private readonly HttpClient _httpClient;
		private readonly string _url = "https://fakestoreapi.com/products";

		public ProductApiHealtCheck()
		{
			_httpClient = new HttpClient();
		}

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			try
			{
				var response = await _httpClient.GetAsync(_url, cancellationToken);
				if (response.IsSuccessStatusCode)
				{
					return HealthCheckResult.Healthy("Product API is healthy.");
				}
				return HealthCheckResult.Healthy($"Product API is not healthy, status: {response.StatusCode}");
			}
			catch (Exception ex)
			{
				return HealthCheckResult.Unhealthy("Catalog API is unhealthy.", ex);
			}
		}
	}
}
