using Store.EndToEndTest.Api.User.Common;
using Store.Tests.Shared;

namespace Store.EndToEndTest.Api.User.DeactivateUser
{
    [CollectionDefinition(nameof(DeactivateUserApiTestFixture))]
    public class DeactivateUserApiTestFixtureCollection
		: ICollectionFixture<DeactivateUserApiTestFixture>
    { }
    public class DeactivateUserApiTestFixture : UserBaseFixture
    {
        public UserDataGenerator DataGenerator { get; }

        public DeactivateUserApiTestFixture() => DataGenerator = new UserDataGenerator();
        public List<Domain.Entity.User> GetExampleUserList(int quantity = 15) => DataGenerator.GetExampleUserList(quantity);
	}
}
