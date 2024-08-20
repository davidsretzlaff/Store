using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Store.Application.Common.Exceptions;
using Store.Domain.ValueObject;
using Store.Infra.Data.EF;
using Store.Infra.Data.EF.Repositories;
using ApplicationUseCases = Store.Application.UseCases.User.CreateUser;
using DomainEntity = Store.Domain.Entity;

namespace Store.IntegrationTest.Application.User.CreateUser
{
	[Collection(nameof(CreateUserTestFixture))]
	public  class CreateUserTest
	{

		private readonly CreateUserTestFixture _fixture;

		public CreateUserTest(CreateUserTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(CeateUser))]
		[Trait("Integration/Application", "CeateUser - Use Cases")]
		public async void CeateUser()
		{
			// Arrange
			var dbContext = _fixture.CreateDbContext();
			var repository = new UserRepository(dbContext);
			var unitOfWork = new UnitOfWork(dbContext);
			var useCase = new ApplicationUseCases.CreateUser(repository, unitOfWork);
			var input = _fixture.CreateUserInput();

			// Act
			var output = await useCase.Handle(input, CancellationToken.None);

			// Assert
			output.Should().NotBeNull();
			output.UserName.Should().Be(input.UserName);
			output.BusinessName.Should().Be(input.BusinessName);
			output.CorporateName.Should().Be(input.CorporateName);
			output.Id.Should().NotBeEmpty();
			output.Status.Should().Be("Waiting");

			var dbUsers = await _fixture.CreateDbContext(true).Users.
				FirstOrDefaultAsync(
					x => x.BusinessName.Equals(input.BusinessName), 
					CancellationToken.None
				);
			dbUsers.Should().NotBeNull();
			dbUsers.Email.Should().Be(input.Email);
			dbUsers!.UserName.Should().Be(input.UserName);
			dbUsers.CorporateName.Should().Be(input.CorporateName);
			dbUsers.CompanyIdentificationNumber.Value.Should().Be(Cnpj.RemoveNonDigits(input.Cnpj));

		}

		[Fact(DisplayName = nameof(Error_When_UserNameExists))]
		[Trait("Integration/Application", "CeateUser - Use Cases")]
		public async Task Error_When_UserNameExists()
		{
			// Arrange
			var dbContext = _fixture.CreateDbContext();
			var repository = new UserRepository(dbContext);
			var unitOfWork = new UnitOfWork(dbContext);
			var useCase = new ApplicationUseCases.CreateUser(repository, unitOfWork);
			var input = _fixture.CreateUserInput();
			var inputInserted = new DomainEntity.User(
				input.UserName,
				input.Password,
				input.BusinessName,
				input.CorporateName,
				input.Email,
				input.SiteUrl,
				input.Phone,
				input.Cnpj,
				input.Address.ToDomainAddress()
			);
			await dbContext.Users.AddAsync(inputInserted, CancellationToken.None);

			// Act
			var action = async () => await useCase.Handle(input, CancellationToken.None);

			// Assert
			action.Should().ThrowAsync<UserNameExistsException>().WithMessage($"'{input.UserName}' already exists.");
		}

		[Fact(DisplayName = nameof(Error_When_CNPJExists))]
		[Trait("Integration/Application", "CeateUser - Use Cases")]
		public async Task Error_When_CNPJExists()
		{
			// Arrange
			var dbContext = _fixture.CreateDbContext();
			var repository = new UserRepository(dbContext);
			var unitOfWork = new UnitOfWork(dbContext);
			var useCase = new ApplicationUseCases.CreateUser(repository, unitOfWork);
			var input = _fixture.CreateUserInput();
			var inputInserted = new DomainEntity.User(
				input.UserName + "other",
				input.Password,
				input.BusinessName,
				input.CorporateName,
				input.Email,
				input.SiteUrl,
				input.Phone,
				input.Cnpj,
				input.Address.ToDomainAddress()
			);
			await dbContext.Users.AddAsync(inputInserted, CancellationToken.None);

			// Act
			var action = async () => await useCase.Handle(input, CancellationToken.None);

			// Assert
			action.Should().ThrowAsync<UserNameExistsException>().WithMessage($"'{input.Cnpj}' already exists.");
		}
	}
}
