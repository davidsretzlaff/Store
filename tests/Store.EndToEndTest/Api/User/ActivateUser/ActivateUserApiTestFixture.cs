using Store.Application.UseCases.User.ActivateUser;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateUser;
using Store.EndToEndTest.Api.User.Common;
using Store.Tests.Shared;

namespace Store.EndToEndTest.Api.User.ActivateUser
{
    [CollectionDefinition(nameof(ActivateUserApiTestFixture))]
    public class ActivateUserApiTestFixtureCollection
        : ICollectionFixture<ActivateUserApiTestFixture>
    { }
    public class ActivateUserApiTestFixture : UserBaseFixture
    {
        public UserDataGenerator DataGenerator { get; }

        public ActivateUserApiTestFixture() => DataGenerator = new UserDataGenerator();
        public List<Domain.Entity.User> GetExampleUserList(int quantity = 15) => DataGenerator.GetExampleUserList(quantity);
	}
}
