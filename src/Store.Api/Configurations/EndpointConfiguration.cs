using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

namespace Store.Api.Configurations
{
	public static class EndpointConfigurationExtensions
	{
		public static IApplicationBuilder UseCustomEndpoints(this IApplicationBuilder app)
		{
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHealthChecks("/health", new HealthCheckOptions
				{
					Predicate = _ => true, 
					ResponseWriter = async (context, report) =>
					{
						context.Response.ContentType = "application/json";
						var result = new
						{
							status = report.Status.ToString(),
							checks = report.Entries.Select(e => new
							{
								name = e.Key,
								status = e.Value.Status.ToString(),
								description = e.Value.Description,
								exception = e.Value.Exception?.Message
							})
						};
						await context.Response.WriteAsync(JsonSerializer.Serialize(result));
					}
				}).AllowAnonymous();

				endpoints.MapGet("/", () => "Hello World!").AllowAnonymous();
			});

			return app;
		}
	}
}
