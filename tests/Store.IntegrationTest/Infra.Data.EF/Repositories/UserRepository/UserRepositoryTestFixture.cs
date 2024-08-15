
using Bogus.Extensions.Brazil;
using Store.Tests.Shared;

namespace Store.IntegrationTest.Infra.Data.EF.Repositories.UserRepository
{
	[CollectionDefinition(nameof(UserRepositoryTestFixture))]
	public class UserRepositoryTestFixtureCollection
	: ICollectionFixture<UserRepositoryTestFixture>
	{ }


	public class UserRepositoryTestFixture : BaseFixture
	{
		public UserDataGenerator DataGenerator { get; }
		public UserRepositoryTestFixture() => DataGenerator = new UserDataGenerator();
		public Domain.Entity.User GetValidUser() => DataGenerator.GetValidUser();
		public List<Domain.Entity.User> GetUserValidList(int quantity) => DataGenerator.GetUserValidList(quantity);
	}
}
