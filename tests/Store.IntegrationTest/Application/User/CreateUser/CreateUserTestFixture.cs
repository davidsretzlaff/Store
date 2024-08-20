
using Store.Application.UseCases.User.CreateUser;
using Store.Tests.Shared;

namespace Store.IntegrationTest.Application.User.CreateUser
{


	[CollectionDefinition(nameof(CreateUserTestFixture))]
	public class CreateUserTestFixtureCollection : ICollectionFixture<CreateUserTestFixture>
	{ }

	public class CreateUserTestFixture : BaseFixture
	{
		public UserDataGenerator _dataGenerator;
		public CreateUserTestFixture() => _dataGenerator = new UserDataGenerator();
		public CreateUserInput CreateUserInput() => _dataGenerator.CreateUserInput();
	}
}
