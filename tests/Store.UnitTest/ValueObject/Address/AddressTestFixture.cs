using Store.Tests.Shared;

namespace Store.UnitTest.ValueObject
{
	[CollectionDefinition(nameof(AddressTestFixture))]
	public class AddressTestFixtureCollection : ICollectionFixture<AddressTestFixture> { }

	public class AddressTestFixture
	{
		public AddressDataGenerator DataGenerator { get; }
		public AddressTestFixture() => DataGenerator = new AddressDataGenerator();
		public Domain.ValueObject.Address GetValidAddress() => DataGenerator.GetValidAddress();
	}
}
