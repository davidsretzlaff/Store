using FluentAssertions;
using Store.Domain.Exceptions;
using Store.UnitTest.ValueObject;

namespace Store.UnitTest.Entity
{
	[Collection(nameof(UserTestFixture))]
	public class UserTest
	{
		private readonly UserTestFixture _fixture;
		public UserTest(UserTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(Instantiate))]
		[Trait("Domain", "User - Entity")]
		public void Instantiate()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var user = new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			user.Should().NotBeNull();
			user.BusinessName.Should().Be(validUser.BusinessName);
			user.CorporateName.Should().Be(validUser.CorporateName);
			user.Status.Should().Be(validUser.Status);
			user.Email.Should().Be(validUser.Email);
			user.SiteUrl.Should().Be(validUser.SiteUrl);
			user.Phone.Should().Be(validUser.Phone);
			user.CompanyRegistrationNumber.Should().Be(validUser.CompanyRegistrationNumber);
			user.Address.Should().NotBeNull();
			user.Address.Street.Should().Be(validUser.Address.Street);
			user.Address.City.Should().Be(validUser.Address.City);
			user.Address.State.Should().Be(validUser.Address.State);
			user.Address.Country.Should().Be(validUser.Address.Country);
			user.Address.ZipCode.Should().Be(validUser.Address.ZipCode);
		}

		[Fact(DisplayName = nameof(CreateUser_ShouldStartAsWaiting))]
		[Trait("Domain", "User - Entity")]
		public void CreateUser_ShouldStartAsWaiting()
		{
			// Arrange
			Domain.Entity.User user;

			// Act
			user = _fixture.GetValidUser();

			// Assert
			user.Should().NotBeNull();
			user.Status.Should().Be(Domain.Enum.UserStatus.Waiting);
		}

		[Fact(DisplayName = nameof(ActivateUser_ShouldBeActive))]
		[Trait("Domain", "User - Entity")]
		public void ActivateUser_ShouldBeActive()
		{
			// Arrange
			var user = _fixture.GetValidUser();

			// Act
			user.Activate();

			// Assert
			user.Should().NotBeNull();
			user.Status.Should().Be(Domain.Enum.UserStatus.Active);
		}

		[Fact(DisplayName = nameof(DeactiveUser_ShouldBeInactive))]
		[Trait("Domain", "User - Entity")]
		public void DeactiveUser_ShouldBeInactive()
		{
			// Arrange
			var user = _fixture.GetValidUser();

			// Act
			user.Deactivate();

			// Assert
			user.Should().NotBeNull();
			user.Status.Should().Be(Domain.Enum.UserStatus.Inactive);
		}

		[Fact(DisplayName = nameof(ThrowError_When_BusinessNameIsInvalid))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_BusinessNameIsInvalid()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				null!,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"BusinessName should not be empty or null");
		}

		[Theory(DisplayName = nameof(ThrowError_When_BusinessNameIsInvalid))]
		[Trait("Domain", "User - Entity")]
		[InlineData("ab")]
		[InlineData("a")]
		public void ThrowError_When_BusinessNameIsLessThan4Characters(string invalidBusinessName)
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				invalidBusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"BusinessName should be at least 4 characters long");
		}

		[Fact(DisplayName = nameof(ThrowError_When_BusinessNameIsGreaterThan100Characters))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_BusinessNameIsGreaterThan100Characters()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();
			var invalidBusinessName = string.Join(null, Enumerable.Range(1, 101).Select(_ => "a").ToArray());

			// Act
			var action = () => new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				invalidBusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"BusinessName should be less or equal 100 characters long");
		}

		[Fact(DisplayName = nameof(ThrowError_When_CorporateNameIsInvalid))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_CorporateNameIsInvalid()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				validUser.BusinessName,
				null!,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"CorporateName should not be empty or null");
		}

		[Theory(DisplayName = nameof(ThrowError_When_CorporateIsLessThan3Characters))]
		[Trait("Domain", "User - Entity")]
		[InlineData("ab")]
		[InlineData("a")]
		public void ThrowError_When_CorporateIsLessThan3Characters(string invalidCorporateName)
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				validUser.BusinessName,
				invalidCorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"CorporateName should be at least 3 characters long");
		}

		[Fact(DisplayName = nameof(ThrowError_When_CorporateNameIsGreaterThan100Characters))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_CorporateNameIsGreaterThan100Characters()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();
			var invalidCorporateName = string.Join(null, Enumerable.Range(1, 101).Select(_ => "a").ToArray());

			// Act
			var action = () => new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				validUser.BusinessName,
				invalidCorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"CorporateName should be less or equal 100 characters long");
		}

		[Fact(DisplayName = nameof(ThrowError_When_URLIsInvalid))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_URLIsInvalid()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				"",
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"SiteUrl should not be empty or null");
		}

		[Fact(DisplayName = nameof(ThrowError_When_URLIsInvalid))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_CompanyRegistrationNumberIsInvalid()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				null!,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"CompanyRegistrationNumber should not be empty or null");
		}

		[Fact(DisplayName = nameof(ThrowError_When_URLIsInvalid))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_EmailIsInvalid()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				validUser.BusinessName,
				validUser.CorporateName,
				"invalid",
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"Email invalid");
		}

		[Fact(DisplayName = nameof(ThrowError_When_PhoneIsInvalid))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_PhoneIsInvalid()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				validUser.UserName,
				validUser.Password,
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				"invalid",
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"Phone invalid");
		}

		[Fact(DisplayName = nameof(ThrowError_When_NameIsInvalid))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_NameIsInvalid()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				null!,
				validUser.Password,
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"UserName should not be empty or null");
		}

		[Theory(DisplayName = nameof(ThrowError_When_NameIsLessThan4Characters))]
		[Trait("Domain", "User - Entity")]
		[InlineData("ab")]
		[InlineData("a")]
		public void ThrowError_When_NameIsLessThan4Characters(string invalidName)
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				invalidName,
				validUser.Password,
				validUser.UserName,
				validUser.BusinessName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"UserName should be at least 4 characters long");
		}

		[Fact(DisplayName = nameof(ThrowError_When_NameIsGreaterThan100Characters))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_NameIsGreaterThan100Characters()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();
			var invalidName = string.Join(null, Enumerable.Range(1, 101).Select(_ => "a").ToArray());

			// Act
			var action = () => new Domain.Entity.User(
				invalidName,
				validUser.Password,
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"UserName should be less or equal 100 characters long");
		}

		[Fact(DisplayName = nameof(ThrowError_When_NameIsGreaterThan100Characters))]
		[Trait("Domain", "User - Entity")]
		public void ThrowError_When_UserNameHasSpaceEmpty()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var action = () => new Domain.Entity.User(
				"Username With Space",
				validUser.Password,
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address
			 );

			// Assert
			action.Should().Throw<EntityValidationException>().WithMessage($"Username should not contain spaces");
		}
	}
}
