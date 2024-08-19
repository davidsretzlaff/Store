using Store.Application.UseCases.User.CreateUser;
using Store.Infra.Data.EF;
using Store.Infra.Data.EF.Repositories;
using MediatR;
using Store.Infra.Adapters.Identity;
using Store.Application.UseCases.Order.CreateOrder;
using Store.Infra.Adapters.ExternalCatalog.Repositories;
using Store.Infra.Adapters.ExternalCatalog;
using Store.Infra.Adapters.CacheService;
using Store.Domain.Interface.Application;
using Store.Application.Common.UserValidation;
using Store.Domain.Interface.Infra.Adapters;
using Store.Domain.Interface.Infra.Repository;

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
			services.AddTransient<IProductService, ProductService>(); 
			services.AddSingleton<ICacheService, CacheService>();
			services.AddTransient<IUserValidation, UserValidation>();
			services.AddTransient<IDeliveryRepository, DeliveryRepository>();
			services.AddHttpClient<ApiClient>(client =>
			{
				client.BaseAddress = new Uri("https://fakestoreapi.com");
			});

			return services;
		}
	}
}
