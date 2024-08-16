using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateUser;
using Store.EndToEndTest.Api.User.Common;
using Store.Tests.Shared;

namespace Store.EndToEndTest.Api.User.CreateUser
{
	public class CreateUserApiTestFixtureCollection : ICollectionFixture<CreateUserApiTestFixture> { }
	public class CreateUserApiTestFixture : UserBaseFixture
	{
		public UserDataGenerator DataGenerator { get; }
		public CreateUserApiTestFixture() => DataGenerator = new UserDataGenerator();
		public CreateUserInput getExampleInput()
		{
			var user = DataGenerator.GetValidUser();
			return new(
				user.Name,
				user.BusinessName,
				user.CorporateName,
				user.Email,
				user.SiteUrl,
				user.Phone,
				user.CompanyRegistrationNumber,
				AddressInput.FromDomainAddress(user.Address)
			);
		}
	}
}
