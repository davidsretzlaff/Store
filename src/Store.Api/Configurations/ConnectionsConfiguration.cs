using Microsoft.EntityFrameworkCore;
using Store.Infra.Data.EF;

namespace Store.Api.Configurations
{
	public static class ConnectionsConfiguration
	{
		public static IServiceCollection AddAppConections(
			this IServiceCollection services,
			IConfiguration configuration
		)
		{
			services.AddDbConnection(configuration);
			return services;
		}

		private static IServiceCollection AddDbConnection(
			this IServiceCollection services,
			IConfiguration configuration
		)
		{
			var connectionString = configuration
				.GetConnectionString("CatalogDb");
			services.AddDbContext<StoreDbContext>(
				options => options.UseInMemoryDatabase("inMemory-Database")
				
			);
			return services;
		}
	}
}
