using Store.IntegrationTest.Infra.Data.EF.Repositories.UserRepository;
using Store.Tests.Shared;

namespace Store.IntegrationTest.Infra.Data.EF.UnitOfWork
{

	[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
	public class UnitOfWorkTestFixtureCollection
	: ICollectionFixture<UnitOfWorkTestFixture>
	{ }
	public class UnitOfWorkTestFixture : BaseFixture
	{
		public UserDataGenerator DataGenerator { get; }
		public UnitOfWorkTestFixture() => DataGenerator = new UserDataGenerator();
		public List<Domain.Entity.User> GetUserValidList(int quantity) => DataGenerator.GetUserValidList(quantity);
	}
}
