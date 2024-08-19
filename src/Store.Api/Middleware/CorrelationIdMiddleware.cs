namespace Store.Api.Middleware
{
	public class CorrelationIdMiddleware
	{
		private readonly RequestDelegate _next;

		public CorrelationIdMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			const string CorrelationIdHeader = "Correlation-Id";

			if (!context.Request.Headers.ContainsKey(CorrelationIdHeader))
			{
				context.Request.Headers[CorrelationIdHeader] = Guid.NewGuid().ToString();
			}

			context.Response.OnStarting(() =>
			{
				context.Response.Headers[CorrelationIdHeader] = context.Request.Headers[CorrelationIdHeader];
				return Task.CompletedTask;
			});

			await _next(context);
		}
	}
}


