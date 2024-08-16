using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Store.Api.Exception;
using System.Text;

namespace Store.Api.Configurations
{
	public static class ControllerConfiguration
	{
		public static IServiceCollection AddAndConfigureControllers(this IServiceCollection services, IConfiguration configuration)
		{

			var key = Encoding.ASCII.GetBytes(configuration["JwtSettings:SecretKey"]);

			services.AddCors(p => p.AddPolicy("CORS", builder =>
			{
				builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
			}))
			.AddControllers(options =>
			{
				options.Filters.Add(typeof(ApiGlobalExceptionFilter));
			});

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
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
