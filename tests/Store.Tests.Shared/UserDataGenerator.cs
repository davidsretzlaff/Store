using Bogus;
using Bogus.Extensions.Brazil;

namespace Store.Tests.Shared
{
	public class UserDataGenerator : BaseFixture
	{
		public Domain.Entity.User GetValidUser()
		{

			return new Domain.Entity.User(
				Faker.Person.FullName,
				Faker.Company.CompanyName(),
				Faker.Company.CompanyName(),
				Faker.Person.Email,
				"www.sitecompany.com.br",
				"55 992364499",
				Faker.Company.Cnpj(),
				GetEmail()
			);
		}

		private Domain.ValueObject.Address GetEmail()
		{
			return new Domain.ValueObject.Address(
				Faker.Address.StreetName(),
				Faker.Address.City(),
				Faker.Address.State(),
				Faker.Address.Country(),
				Faker.Address.ZipCode()
			);
		}

		public List<Domain.Entity.User> GetUserValidList(int quantity)
		{
			return Enumerable.Range(1, quantity).Select(_ => GetValidUser()).ToList();
		}
	}
}
