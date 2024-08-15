using Bogus.Extensions.Brazil;
using Store.UnitTest.Common.Fixture;
using Store.UnitTest.ValueObject;

namespace Store.UnitTest.Entity
{
	public class UserTestFixture : BaseFixture
	{
		public Domain.Entity.User GetValidUser()
		{
			return new Domain.Entity.User(
				Faker.Company.CompanyName(),
				Faker.Company.CompanyName(),
				Domain.Enum.UserStatus.Waiting,
				Faker.Person.Email,
				"www.sitecompany.com.br",
				Faker.Person.Phone,
				Faker.Company.Cnpj(),
				Faker.Address.StreetName(),
				Faker.Address.City(),
				Faker.Address.State(),
				Faker.Address.Country(),
				Faker.Address.ZipCode()
			);
		}

	}
	[CollectionDefinition(nameof(UserTestFixture))]
	public class UserTestFixtureCollection: ICollectionFixture<UserTestFixture> { }
}
