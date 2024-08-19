using Microsoft.AspNetCore.Authorization;

namespace Store.Api.Middleware
{
	public class IdentityMiddleware
	{
		private readonly RequestDelegate _next;

		public IdentityMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var endpoint = context.GetEndpoint();

			if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
			{
				await _next(context);
				return;
			}

			var user = context.User;

			if (user?.Identity?.IsAuthenticated != true)
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("User is not authenticated.");
				return;
			}

			var registerNumber = user.Claims.FirstOrDefault(c => c.Type == "CompanyRegisterNumber")?.Value;

			if (string.IsNullOrEmpty(registerNumber))
			{
				context.Response.StatusCode = StatusCodes.Status400BadRequest;
				await context.Response.WriteAsync("Register number claim not found.");
				return;
			}

			await _next(context);
		}
	}
}
