namespace Store.Tests.Shared
{
	public class AddressDataGenerator: BaseFixture
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
}
