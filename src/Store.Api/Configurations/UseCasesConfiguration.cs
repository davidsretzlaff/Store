using Store.Application.Interface;
using Store.Application.UseCases.User.CreateUser;
using Store.Domain.Repository;
using Store.Infra.Data.EF;
using Store.Infra.Data.EF.Repositories;
using MediatR;

namespace Store.Api.Configurations
{
	public static class UseCasesConfiguration
	{
		public static IServiceCollection AddUseCases(this IServiceCollection services)
		{
			services.AddHealthChecks();
			services.AddMediatR(typeof(CreateUser));
			services.AddRepositories();
			return services;
		}

		private static IServiceCollection AddRepositories(this IServiceCollection services)
		{
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IUnitOfWork, UnitOfWork>();
			return services;
		}
	}
}
