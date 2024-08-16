using Store.Application.Interface;
using Store.Application.UseCases.User.CreateUser;
using Store.Domain.Repository;
using Store.Infra.Data.EF;
using Store.Infra.Data.EF.Repositories;
using MediatR;
using Store.Application.UseCases.User.CreateAuthenticate;

namespace Store.Api.Configurations
{
	public static class UseCasesConfiguration
	{
		public static IServiceCollection AddUseCases(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHealthChecks();
			services.AddMediatR(typeof(CreateUser));
			services.AddMediatR(typeof(CreateAuth));
			services.AddRepositories(configuration);
			return services;
		}

		private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
		{
			// Carregar a chave secreta do appsettings.json
			var jwtSecretKey = configuration["JwtSettings:SecretKey"];

			// Registrar o CreateAuth com a chave secreta
			services.AddTransient<CreateAuth>(sp =>
			{
				var userRepository = sp.GetRequiredService<IUserRepository>();
				var unitOfWork = sp.GetRequiredService<IUnitOfWork>();
				return new CreateAuth(userRepository, unitOfWork, jwtSecretKey);
			});

			// Registrar MediatR
			services.AddMediatR(typeof(CreateAuth));
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IUnitOfWork, UnitOfWork>();
			return services;
		}
	}
}
