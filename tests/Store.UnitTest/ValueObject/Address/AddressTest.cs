
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

        [Theory(DisplayName = nameof(ThrowError_When_StreetIsInvalid))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void ThrowError_When_StreetIsInvalid(string? invalidStreet)
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

		[Theory(DisplayName = nameof(ThrowError_When_StreetIsLessThan4Characters))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("ab")]
		[InlineData("a")]
		public void ThrowError_When_StreetIsLessThan4Characters(string? invalidStreet)
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

		[Fact(DisplayName = nameof(ThrowError_When_StreetIsGreaterThan100Characters))]
		[Trait("Domain", "Address - ValueObject")]
		public void ThrowError_When_StreetIsGreaterThan100Characters()
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

		[Theory(DisplayName = nameof(ThrowError_When_CityIsInvalid))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData(null)]
		public void ThrowError_When_CityIsInvalid(string? invalidCity)
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

		[Theory(DisplayName = nameof(ThrowError_When_CityIsLessThan3Characters))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("ab")]
		[InlineData("a")]
		public void ThrowError_When_CityIsLessThan3Characters(string? invalidCity)
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

		[Fact(DisplayName = nameof(ThrowError_When_CityIsGreaterThan100Characters))]
		[Trait("Domain", "Address - ValueObject")]
		public void ThrowError_When_CityIsGreaterThan100Characters()
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

		[Theory(DisplayName = nameof(ThrowError_When_StateIsInvalid))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData(null)]
		public void ThrowError_When_StateIsInvalid(string? invalidState)
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

		[Theory(DisplayName = nameof(ThrowError_When_StateIsLessThan1Characters))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
		public void ThrowError_When_StateIsLessThan1Characters(string? invalidState)
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

		[Fact(DisplayName = nameof(ThrowError_When_StateIsGreaterThan100Characters))]
		[Trait("Domain", "Address - ValueObject")]
		public void ThrowError_When_StateIsGreaterThan100Characters()
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

		[Theory(DisplayName = nameof(ThrowError_When_CountryIsInvalid))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData(null)]
		public void ThrowError_When_CountryIsInvalid(string? invalidCountry)
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

		[Fact(DisplayName = nameof(ThrowError_When_CountryIsGreaterThan100Characters))]
		[Trait("Domain", "Address - ValueObject")]
		public void ThrowError_When_CountryIsGreaterThan100Characters()
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

		[Fact(DisplayName = nameof(ThrowError_When_ZipCodeIsInvalid))]
		[Trait("Domain", "Address - ValueObject")]
		public void ThrowError_When_ZipCodeIsInvalid()
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

		[Theory(DisplayName = nameof(ThrowError_When_ZipCodeIsLessThan7Characters))]
		[Trait("Domain", "Address - ValueObject")]
		[InlineData("")]
		public void ThrowError_When_ZipCodeIsLessThan7Characters(string? invalidZipCode)
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

		[Fact(DisplayName = nameof(ThrowError_When_ZipCodeIsGreaterThan8Characters))]
		[Trait("Domain", "Address - ValueObject")]
		public void ThrowError_When_ZipCodeIsGreaterThan8Characters()
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
