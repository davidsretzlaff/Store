using Store.Domain.Enum;
using Store.Tests.Shared;

namespace Store.IntegrationTest.Infra.Data.EF.Repositories.UserRepository
{
	[CollectionDefinition(nameof(UserRepositoryTestFixture))]
	public class UserRepositoryTestFixtureCollection
	: ICollectionFixture<UserRepositoryTestFixture>
	{ }


	public class UserRepositoryTestFixture : BaseFixture
	{
		public UserDataGenerator _dataGenerator { get; }
		public UserRepositoryTestFixture() => _dataGenerator = new UserDataGenerator();
		public Domain.Entity.User GetValidUser() => _dataGenerator.GetValidUser();
		public List<Domain.Entity.User> GetUserValidList(int quantity) => _dataGenerator.GetUserValidList(quantity);
		public List<Domain.Entity.User> GetExampleListUsersByNames(List<string[]> inputs)
			=> _dataGenerator.GetExampleListUsersByNames(inputs);

		public List<Domain.Entity.User> CloneUserListOrdered(
			List<Domain.Entity.User> userList, 
			string orderBy, 
			SearchOrder order
		) => _dataGenerator.CloneUserListOrdered(userList, orderBy, order);
	}
}
