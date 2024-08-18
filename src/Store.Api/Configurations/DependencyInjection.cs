using Store.Application.UseCases.User.CreateUser;
using Store.Domain.Repository;
using Store.Infra.Data.EF;
using Store.Infra.Data.EF.Repositories;
using MediatR;
using Store.Infra.Adapters.Identity;
using Store.Application.Common.Interface;
using Store.Application.UseCases.Order.CreateOrder;
using Store.Infra.Adapters.ExternalCatalog.Repositories;
using Store.Infra.Adapters.ExternalCatalog;

namespace Store.Api.Configurations
{
    public static class DependencyInjection
	{
		public static IServiceCollection addApiServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHealthChecks();
			services.AddMediatR(typeof(CreateUser));
			services.AddMediatR(typeof(CreateOrder));
			//services.AddMediatR(typeof(CreateAuth));
			services.AddScoped<IJwtUtils, JwtUtils>();
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IOrderRepository, OrderRepository>();
			services.AddTransient<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IApiClient, ApiClient>();
			services.AddSingleton<IProductRepository, ProductRepository>();
			
			services.AddHttpClient<ApiClient>(client =>
			{
				client.BaseAddress = new Uri("https://fakestoreapi.com");
			});

			// Register ProductRepository

			return services;
		}
	}
}
