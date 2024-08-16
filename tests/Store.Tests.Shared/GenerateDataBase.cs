using Microsoft.EntityFrameworkCore;
using Store.Infra.Data.EF;

namespace Store.Tests.Shared
{
	public class GenerateDataBase
	{
		public StoreDbContext CreateDbContext(string databaseName, bool preserveData = false)
		{
			var context = new StoreDbContext(
				new DbContextOptionsBuilder<StoreDbContext>()
				.UseInMemoryDatabase(databaseName)
				.Options
			);

			if (preserveData == false)
				context.Database.EnsureDeleted();

			return context;
		}
	}
}
