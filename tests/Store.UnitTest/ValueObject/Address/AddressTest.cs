
using FluentAssertions;
using Store.Domain.Exceptions;

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

        [Theory(DisplayName = nameof(ThrowErrorWhenStreetIsInvalid))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void ThrowErrorWhenStreetIsInvalid(string? invalidStreet)
        {
			// Arrange
			var validAddress = _fixture.GetValidAddress();

			// Act
			var action = () => new Domain.ValueObject.Address(
				invalidStreet,
				validAddress.City,
				validAddress.State,
				validAddress.Country,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"Street should not be empty or null");
		}

		[Theory(DisplayName = nameof(InstantiateErrorWhenStreetIsLessThan4Characters))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("ab")]
		[InlineData("a")]
		public void InstantiateErrorWhenStreetIsLessThan4Characters(string? invalidStreet)
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();

			// Act
			var action = () => new Domain.ValueObject.Address(
				invalidStreet!,
				validAddress.City,
				validAddress.State,
				validAddress.Country,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"Street should be at least 4 characters long");
		}

		[Fact(DisplayName = nameof(InstantiateErrorWhenStreetIsGreaterThan100Characters))]
		[Trait("Domain", "Address - ValueObject")]
		public void InstantiateErrorWhenStreetIsGreaterThan100Characters()
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();
			var invalidStreet = string.Join(null, Enumerable.Range(1, 101).Select(_ => "a").ToArray());
			// Act
			var action = () => new Domain.ValueObject.Address(
				invalidStreet,
				validAddress.City,
				validAddress.State,
				validAddress.Country,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"Street should be less or equal 100 characters long");
		}

		[Theory(DisplayName = nameof(ThrowErrorWhenCityIsInvalid))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData(null)]
		public void ThrowErrorWhenCityIsInvalid(string? invalidCity)
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();

			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				invalidCity!,
				validAddress.State,
				validAddress.Country,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"City should not be empty or null");
		}

		[Theory(DisplayName = nameof(InstantiateErrorWhenCityIsLessThan3Characters))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("ab")]
		[InlineData("a")]
		public void InstantiateErrorWhenCityIsLessThan3Characters(string? invalidCity)
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();

			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				invalidCity!,
				validAddress.State,
				validAddress.Country,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"City should be at least 3 characters long");
		}

		[Fact(DisplayName = nameof(InstantiateErrorWhenCityIsGreaterThan100Characters))]
		[Trait("Domain", "Address - ValueObject")]
		public void InstantiateErrorWhenCityIsGreaterThan100Characters()
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();
			var invalidCity = string.Join(null, Enumerable.Range(1, 101).Select(_ => "a").ToArray());
			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				invalidCity,
				validAddress.State,
				validAddress.Country,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"City should be less or equal 100 characters long");
		}

		[Theory(DisplayName = nameof(ThrowErrorWhenStateIsInvalid))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData(null)]
		public void ThrowErrorWhenStateIsInvalid(string? invalidState)
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();

			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				validAddress.City,
				invalidState!,
				validAddress.Country,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"State should not be empty or null");
		}

		[Theory(DisplayName = nameof(InstantiateErrorWhenStateIsLessThan1Characters))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
		public void InstantiateErrorWhenStateIsLessThan1Characters(string? invalidState)
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();

			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				validAddress.City,
				invalidState!,
				validAddress.Country,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"State should not be empty or null");
		}

		[Fact(DisplayName = nameof(InstantiateErrorWhenStateIsGreaterThan100Characters))]
		[Trait("Domain", "Address - ValueObject")]
		public void InstantiateErrorWhenStateIsGreaterThan100Characters()
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();
			var invalidState = string.Join(null, Enumerable.Range(1, 101).Select(_ => "a").ToArray());

			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				validAddress.City,
				invalidState,
				validAddress.Country,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"State should be less or equal 100 characters long");
		}

		[Theory(DisplayName = nameof(ThrowErrorWhenCountryIsInvalid))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData(null)]
		public void ThrowErrorWhenCountryIsInvalid(string? invalidCountry)
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();

			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				validAddress.City,
				validAddress.Street,
				invalidCountry,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"Country should not be empty or null");
		}

		[Fact(DisplayName = nameof(InstantiateErrorWhenCountryIsGreaterThan100Characters))]
		[Trait("Domain", "Address - ValueObject")]
		public void InstantiateErrorWhenCountryIsGreaterThan100Characters()
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();
			var invalidCountry = string.Join(null, Enumerable.Range(1, 101).Select(_ => "a").ToArray());

			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				validAddress.City,
				validAddress.Street,
				invalidCountry,
				validAddress.ZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"Country should be less or equal 100 characters long");
		}

		[Fact(DisplayName = nameof(ThrowErrorWhenZipCodeIsInvalid))]
		[Trait("Domain", "Address - ValueObject")]
		public void ThrowErrorWhenZipCodeIsInvalid()
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();

			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				validAddress.City,
				validAddress.State,
				validAddress.Country,
				null!
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"ZipCode should not be null");
		}

		[Theory(DisplayName = nameof(InstantiateErrorWhenStateIsLessThan1Characters))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
		public void InstantiateErrorWhenZipCodeIsLessThan7Characters(string? invalidZipCode)
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();

			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				validAddress.City,
				validAddress.State,
				validAddress.Country,
				invalidZipCode!
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"ZipCode should be at least 8 characters long");
		}

		[Fact(DisplayName = nameof(InstantiateErrorWhenZipCodeIsGreaterThan8Characters))]
		[Trait("Domain", "Address - ValueObject")]
		public void InstantiateErrorWhenZipCodeIsGreaterThan8Characters()
		{
			// Arrange
			var validAddress = _fixture.GetValidAddress();
			var invalidZipCode = "86116-1111";

			// Act
			var action = () => new Domain.ValueObject.Address(
				validAddress.Street,
				validAddress.City,
				validAddress.State,
				validAddress.Country,
				invalidZipCode
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"ZipCode should be less or equal 8 characters long");
		}
	}
}
