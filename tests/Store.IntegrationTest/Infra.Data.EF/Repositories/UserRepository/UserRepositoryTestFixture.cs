
using Bogus.Extensions.Brazil;

namespace Store.IntegrationTest.Infra.Data.EF.Repositories.UserRepository
{
	[CollectionDefinition(nameof(UserRepositoryTestFixture))]
	public class UserRepositoryTestFixtureCollection
	: ICollectionFixture<UserRepositoryTestFixture>
	{ }


	public class UserRepositoryTestFixture : BaseFixture
	{
		public Domain.Entity.User GetValidUser()
		{
			return new Domain.Entity.User(
				Faker.Company.CompanyName(),
				Faker.Company.CompanyName(),
				Faker.Person.Email,
				"www.sitecompany.com.br",
				"55 992364499",
				Faker.Company.Cnpj(),
				Faker.Address.StreetName(),
				Faker.Address.City(),
				Faker.Address.State(),
				Faker.Address.Country(),
				Faker.Address.ZipCode()
			);

			//new Domain.ValueObject.Address(
			//		Faker.Address.StreetName(),
			//		Faker.Address.City(),
			//		Faker.Address.State(),
			//		Faker.Address.State(),
			//		Faker.Address.ZipCode()
			//	)
		}
	}
}
