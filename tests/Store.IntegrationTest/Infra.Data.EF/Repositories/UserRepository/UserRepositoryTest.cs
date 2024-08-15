using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Repository;
using Repository = Store.Infra.Data.EF.Repositories;

namespace Store.IntegrationTest.Infra.Data.EF.Repositories.UserRepository
{
	[Collection(nameof(UserRepositoryTestFixture))]
	public class UserRepositoryTest
	{
		private readonly UserRepositoryTestFixture _fixture;
		public UserRepositoryTest(UserRepositoryTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(Insert))]
		[Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
		public async Task Insert()
		{
			var user = _fixture.GetValidUser();
			var context = _fixture.CreateDbContext();
			var repository = new Repository.UserRepository(context);

			await repository.Insert(user, CancellationToken.None);
			context.SaveChanges();

			// persist state
			var assertionContext = _fixture.CreateDbContext(true);
			var userFromDb = await assertionContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.Id);
			userFromDb.Should().NotBeNull();
			userFromDb!.BusinessName.Should().Be(user.BusinessName);
			userFromDb.Status.Should().Be(user.Status);
			userFromDb.CompanyRegistrationNumber.Should().Be(user.CompanyRegistrationNumber);
			userFromDb.CorporateName.Should().Be(user.CorporateName);
			userFromDb.Email.Should().Be(user.Email);
			userFromDb.Phone.Should().Be(user.Phone);
			userFromDb.SiteUrl.Should().Be(user.SiteUrl);
			userFromDb.Address.Should().NotBeNull();
			userFromDb.Address.Street.Should().Be(user.Address.Street);
			userFromDb.Address.City.Should().Be(user.Address.City);
			userFromDb.Address.Country.Should().Be(user.Address.Country);
			userFromDb.Address.State.Should().Be(user.Address.State);
			userFromDb.Address.ZipCode.Should().Be(user.Address.ZipCode);
		}
	}
}
