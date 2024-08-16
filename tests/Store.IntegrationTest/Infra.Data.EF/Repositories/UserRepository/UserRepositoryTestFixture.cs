using Store.Domain.Entity;
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

		private Domain.Entity.User GetExampleUser(string[] inputs = null)
		{
			var user = GetValidUser();
			return new Domain.Entity.User(inputs[0], inputs[1], inputs[2],user.Email,user.SiteUrl,user.Phone,user.CompanyRegistrationNumber,user.Address);
		}
		public List<Domain.Entity.User> GetExampleListUsersByNames(List<string[]> inputs)
			=> inputs.Select(input => GetExampleUser(inputs: input)).ToList();
	}
}
