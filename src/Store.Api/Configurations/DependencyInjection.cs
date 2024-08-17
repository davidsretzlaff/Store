using Store.Application.Interface;
using Store.Application.UseCases.User.CreateUser;
using Store.Domain.Repository;
using Store.Infra.Data.EF;
using Store.Infra.Data.EF.Repositories;
using MediatR;
using Store.Infra.Adapters.Identity;

namespace Store.Api.Configurations
{
	public static class DependencyInjection
	{
		public static IServiceCollection addApiServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHealthChecks();
			services.AddMediatR(typeof(CreateUser));
			//services.AddMediatR(typeof(CreateAuth));
			services.AddScoped<IJwtUtils, JwtUtils>();
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IUnitOfWork, UnitOfWork>();
			return services;
		}
	}
}
