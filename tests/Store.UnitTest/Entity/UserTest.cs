﻿using FluentAssertions;
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
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address.Street,
				validUser.Address.City,
				validUser.Address.State,
				validUser.Address.Country,
				validUser.Address.ZipCode
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
			var validUser = _fixture.GetValidUser();

			// Act
			var user = new Domain.Entity.User(
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address.Street,
				validUser.Address.City,
				validUser.Address.State,
				validUser.Address.Country,
				validUser.Address.ZipCode
			 );

			// Assert
			user.Should().NotBeNull();
			user.Status.Should().Be(Domain.Enum.UserStatus.Waiting);
		}

		[Fact(DisplayName = nameof(ActivateUser_ShouldBeActive))]
		[Trait("Domain", "User - Entity")]
		public void ActivateUser_ShouldBeActive()
		{
			// Arrange
			var validUser = _fixture.GetValidUser();

			// Act
			var user = new Domain.Entity.User(
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address.Street,
				validUser.Address.City,
				validUser.Address.State,
				validUser.Address.Country,
				validUser.Address.ZipCode
			 );
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
			var validUser = _fixture.GetValidUser();

			// Act
			var user = new Domain.Entity.User(
				validUser.BusinessName,
				validUser.CorporateName,
				validUser.Email,
				validUser.SiteUrl,
				validUser.Phone,
				validUser.CompanyRegistrationNumber,
				validUser.Address.Street,
				validUser.Address.City,
				validUser.Address.State,
				validUser.Address.Country,
				validUser.Address.ZipCode
			 );
			user.Deactivate();

			// Assert
			user.Should().NotBeNull();
			user.Status.Should().Be(Domain.Enum.UserStatus.Inactive);
		}
	}
}
