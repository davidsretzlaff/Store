using Store.Tests.Shared;

namespace Store.UnitTest.Entity
{
	[CollectionDefinition(nameof(UserTestFixture))]
	public class UserTestFixtureCollection : ICollectionFixture<UserTestFixture> { }

	public class UserTestFixture 
	{
		public UserDataGenerator DataGenerator { get; }
		public UserTestFixture() => DataGenerator = new UserDataGenerator();
		public Domain.Entity.User GetValidUser() => DataGenerator.GetValidUser();
	}
}
