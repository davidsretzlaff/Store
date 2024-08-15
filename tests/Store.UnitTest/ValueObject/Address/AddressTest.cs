
using FluentAssertions;

namespace Store.UnitTest.ValueObject.Address
{
    [Collection(nameof(AddressTestFixture))]
    public class AddressTest
    {
        private readonly AddressTestFixture _fixture;
        public AddressTest(AddressTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Address - ValueObject")]
        public void Instantiate()
        {
            // Arrange
            var validAddress = _fixture.GetValidAddress();

            // Act
            var address = new Domain.ValueObject.Address(
                validAddress.Street,
                validAddress.City,
                validAddress.State,
                validAddress.Country,
                validAddress.ZipCode
             );

            // Assert
            address.Should().NotBeNull();
            address.Street.Should().Be(validAddress.Street);
            address.City.Should().Be(validAddress.City);
            address.State.Should().Be(validAddress.State);
            address.Country.Should().Be(validAddress.Country);
            address.ZipCode.Should().Be(validAddress.ZipCode);
        }
    }
}
