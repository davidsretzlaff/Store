using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateUser;
using Store.EndToEndTest.Api.User.Common;
using Store.Tests.Shared;

namespace Store.EndToEndTest.Api.User.CreateUser
{
    [CollectionDefinition(nameof(CreateUserApiTestFixture))]
	public class CreateCategoryApiTestFixtureCollection
		: ICollectionFixture<CreateUserApiTestFixture>
	{ }
	public class CreateUserApiTestFixture : UserBaseFixture
	{
		public UserDataGenerator DataGenerator { get; }

		public CreateUserApiTestFixture() => DataGenerator = new UserDataGenerator();
		public CreateUserInput getExampleInput()
		{
			var user = DataGenerator.GetValidUser();
			return new(
				user.UserName,
				user.Password,
				user.BusinessName,
				user.CorporateName,
				user.Email,
				user.SiteUrl,
				user.Phone,
				user.Cnpj.Value,
				AddressInput.FromDomainAddress(user.Address)
			);
		}
		public CreateUserInput getInvalidInput()
		{
			var user = DataGenerator.GetValidUser();
			return new(
				"1",
				user.Password,
				user.BusinessName,
				user.CorporateName,
				user.Email,
				user.SiteUrl,
				user.Phone,
				user.Cnpj.Value,
				AddressInput.FromDomainAddress(user.Address)
			);
		}
	}
}
