using Store.Infra.Data.EF;
using Store.Tests.Shared;

namespace Store.EndToEndTest.Base
{
	public class BaseFixture
	{
		public GenerateDataBase DataGenerator { get; }
		public BaseFixture() => DataGenerator = new GenerateDataBase();
		public StoreDbContext CreateDbContext(bool preserveData = false)
		 => DataGenerator.CreateDbContext("end2end-tests-db", preserveData);
	}
}
