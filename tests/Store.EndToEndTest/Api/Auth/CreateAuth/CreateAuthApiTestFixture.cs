using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateUser;
using Store.EndToEndTest.Api.User.Common;
using Store.Tests.Shared;

namespace Store.EndToEndTest.Api.Auth.CreateAuth
{
    [CollectionDefinition(nameof(CreateAuthApiTestFixture))]
    public class CreateAuthApiTestFixtureCollection
		: ICollectionFixture<CreateAuthApiTestFixture>
    { }
    public class CreateAuthApiTestFixture : UserBaseFixture
    {
        public UserDataGenerator DataGenerator { get; }

        public CreateAuthApiTestFixture() => DataGenerator = new UserDataGenerator();

    }
}
