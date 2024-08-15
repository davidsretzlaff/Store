
using Bogus;
using Microsoft.EntityFrameworkCore;
using Store.Infra.Data.EF;

namespace Store.IntegrationTest
{
	public class BaseFixture
	{
		public BaseFixture() => Faker = new Faker("pt_BR");

		protected Faker Faker { get; set; }

		public StoreDbContext CreateDbContext(bool preserveData = false)
		{
			var context = new StoreDbContext(
				new DbContextOptionsBuilder<StoreDbContext>()
				.UseInMemoryDatabase($"integration-tests-db")
				.Options
			);

			//																												if (preserveData == false)
				//context.Database.EnsureDeleted();

			return context;
		}
	}
}
