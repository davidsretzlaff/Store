using Bogus;
using Store.UnitTest.Common.Fixture;

namespace Store.UnitTest.ValueObject
{
	public class AddressTestFixture : BaseFixture
	{

		public Domain.ValueObject.Address GetValidAddress()
		{
			return new Domain.ValueObject.Address(
				Faker.Address.StreetAddress(),
				Faker.Address.City(),
				Faker.Address.State(),
				Faker.Address.Country(),
				Faker.Address.ZipCode()
			);
		}
	}
	[CollectionDefinition(nameof(AddressTestFixture))]
	public class AddressTestFixtureCollection : ICollectionFixture<AddressTestFixture> { }
}
