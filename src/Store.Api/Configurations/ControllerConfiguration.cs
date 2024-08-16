namespace Store.Api.Configurations
{
	public static class ControllerConfiguration
	{
		public static IServiceCollection AddAndConfigureControllers(this IServiceCollection services)
		{
			services.AddCors(p => p.AddPolicy("CORS", builder =>
				{
					builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
				})
			)
			.AddControllers();
			return services;
		}

		//private static IServiceCollection AddDocumentation(
		//	this IServiceCollection services
		//)
		//{
		//	services.AddEndpointsApiExplorer();
		//	services.AddSwaggerGen();
		//	return services;
		//}

		//public static WebApplication UseDocumentation(
		//	this WebApplication app
		//)
		//{
		//	if (app.Environment.IsDevelopment())
		//	{
		//		app.UseSwagger();
		//		app.UseSwaggerUI();
		//	}
		//	return app;
		//}
	}
}
