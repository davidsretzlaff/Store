using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.Infra.Data.EF;

namespace Store.EndToEndTest.Base
{
	public class CustomWebApplicationFactory<TStartup>
	   : WebApplicationFactory<TStartup>
	   where TStartup : class
	{
		protected override void ConfigureWebHost(
			IWebHostBuilder builder
		)
		{
			builder.UseEnvironment("EndToEndTest");
			builder.ConfigureServices(services =>
			{
				var dbOptions = services.FirstOrDefault(
					x => x.ServiceType == typeof(DbContextOptions<StoreDbContext>));
				if ( dbOptions is not null )
				{
					services.Remove( dbOptions );
				}
				services.AddDbContext<StoreDbContext>(
					options => { options.UseInMemoryDatabase("end2end-tests-db"); }
					);

				//var serviceProvider = services.BuildServiceProvider();
				//using var scope = serviceProvider.CreateScope();
				//var context = scope.ServiceProvider
				//	.GetService<CatalogDbContext>();
				//ArgumentNullException.ThrowIfNull(context);
				//context.Database.EnsureDeleted();
				//context.Database.EnsureCreated();
			});

			base.ConfigureWebHost(builder);
		}
	}
}
