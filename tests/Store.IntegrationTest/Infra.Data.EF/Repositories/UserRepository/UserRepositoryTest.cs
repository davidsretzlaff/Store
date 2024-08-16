using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Store.Application.Exceptions;
using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Repository;
using Store.Domain.SeedWork.Searchable;
using Repository = Store.Infra.Data.EF.Repositories;

namespace Store.IntegrationTest.Infra.Data.EF.Repositories.UserRepository
{
	[Collection(nameof(UserRepositoryTestFixture))]
	public class UserRepositoryTest
	{
		private readonly UserRepositoryTestFixture _fixture;
		public UserRepositoryTest(UserRepositoryTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(Insert))]
		[Trait("Integration/Infra.Data", "UserRepository - Repositories")]
		public async Task Insert()
		{
			// Arrange
			var user = _fixture.GetValidUser();
			var context = _fixture.CreateDbContext();
			var repository = new Repository.UserRepository(context);

			// Act
			await repository.Insert(user, CancellationToken.None);
			context.SaveChanges();

			// Assert
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

		[Fact(DisplayName = nameof(Get))]
		[Trait("Integration/Infra.Data", "UserRepository - Repositories")]
		public async Task Get()
		{
			// Arrange
			var userExampleList = _fixture.GetUserValidList(5);
			var user = userExampleList[3];
			var arrangeContext = _fixture.CreateDbContext();
			await arrangeContext.AddRangeAsync(userExampleList);
			await arrangeContext.SaveChangesAsync();
			var repository = new Repository.UserRepository(_fixture.CreateDbContext(true));

			// Act
			var userFromRepository = await repository.Get(user.Id, CancellationToken.None);

			// Assert
			userFromRepository.Should().NotBeNull();
			userFromRepository!.BusinessName.Should().Be(user.BusinessName);
			userFromRepository.Status.Should().Be(user.Status);
			userFromRepository.CompanyRegistrationNumber.Should().Be(user.CompanyRegistrationNumber);
			userFromRepository.CorporateName.Should().Be(user.CorporateName);
			userFromRepository.Email.Should().Be(user.Email);
			userFromRepository.Phone.Should().Be(user.Phone);
			userFromRepository.SiteUrl.Should().Be(user.SiteUrl);
			userFromRepository.Address.Should().NotBeNull();
			userFromRepository.Address.Street.Should().Be(user.Address.Street);
			userFromRepository.Address.City.Should().Be(user.Address.City);
			userFromRepository.Address.Country.Should().Be(user.Address.Country);
			userFromRepository.Address.State.Should().Be(user.Address.State);
			userFromRepository.Address.ZipCode.Should().Be(user.Address.ZipCode);
		}

		[Fact(DisplayName = nameof(Get_ThrowError_When_NotFound))]
		[Trait("Integration/Infra.Data", "UserRepository - Repositories")]
		public async Task Get_ThrowError_When_NotFound()
		{
			// Arrange
			var ramdomGuid = Guid.NewGuid();
			var repository = new Repository.UserRepository(_fixture.CreateDbContext());

			// Act
			var action = async () => await repository.Get(ramdomGuid, CancellationToken.None);

			// Assert
			action.Should().ThrowAsync<NotFoundException>().WithMessage($"User '{ramdomGuid}' not found");
		}


		[Fact(DisplayName = nameof(Search_Return_ListAndTotal))]
		[Trait("Integration/Infra.Data", "UserRepository - Repositories")]
		public async Task Search_Return_ListAndTotal()
		{
			// Arrange
			var dbContext = _fixture.CreateDbContext();
			var exampleUserList = _fixture.GetUserValidList(15);
			await dbContext.AddRangeAsync(exampleUserList);
			await dbContext.SaveChangesAsync(CancellationToken.None);
			var userRepository = new Repository.UserRepository(dbContext);
			var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

			// Act
			var output = await userRepository.Search(searchInput, CancellationToken.None);

			// Assert
			output.Should().NotBeNull();
			output.Items.Should().NotBeNull();
			output.CurrentPage.Should().Be(searchInput.Page);
			output.PerPage.Should().Be(searchInput.PerPage);
			output.Total.Should().Be(exampleUserList.Count);
			output.Items.Should().HaveCount(exampleUserList.Count);

			foreach (User outputItem in output.Items)
			{
				var exampleItem = exampleUserList.Find(
					category => category.Id == outputItem.Id
				);
				exampleItem.Should().NotBeNull();
				outputItem.Id.Should().Be(exampleItem!.Id);
				outputItem.BusinessName.Should().Be(exampleItem.BusinessName);
				outputItem.CorporateName.Should().Be(exampleItem.CorporateName);
				outputItem.Email.Should().Be(exampleItem.Email);
			}
		}

		[Fact(DisplayName = nameof(Search_Return_Empty_When_PersistenceIsEmpty))]
		[Trait("Integration/Infra.Data", "UserRepository - Repositories")]
		public async Task Search_Return_Empty_When_PersistenceIsEmpty()
		{
			// Arrange
			var dbContext = _fixture.CreateDbContext();
			var userRepository = new Repository.UserRepository(dbContext);
			var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

			// Act
			var output = await userRepository.Search(searchInput, CancellationToken.None);


			// Assert
			output.Should().NotBeNull();
			output.Items.Should().NotBeNull();
			output.CurrentPage.Should().Be(searchInput.Page);
			output.PerPage.Should().Be(searchInput.PerPage);
			output.Total.Should().Be(0);
			output.Items.Should().HaveCount(0);
		}

		[Theory(DisplayName = nameof(Search_Return_Paginated))]
		[InlineData(10, 1, 5, 5)]
		[InlineData(10, 2, 5, 5)]
		[InlineData(7, 2, 5, 2)]
		[InlineData(7, 3, 5, 0)]
		[Trait("Integration/Infra.Data", "UserRepository - Repositories")]
		public async Task Search_Return_Paginated(
			int quantityUserToGenerate,
			int NumberOfPagesToReturn,
			int ItemsPerPage,
			int expectedQuantityItems
		)
		{
			// Arrange
			var dbContext = _fixture.CreateDbContext();
			var exampleUserList = _fixture.GetUserValidList(quantityUserToGenerate);
			await dbContext.AddRangeAsync(exampleUserList);
			await dbContext.SaveChangesAsync(CancellationToken.None);
			var userRepository = new Repository.UserRepository(dbContext);
			var searchInput = new SearchInput(NumberOfPagesToReturn, ItemsPerPage, "", "", SearchOrder.Asc);

			// Act
			var output = await userRepository.Search(searchInput, CancellationToken.None);

			// Assert
			output.Should().NotBeNull();
			output.Items.Should().NotBeNull();
			output.CurrentPage.Should().Be(searchInput.Page);
			output.PerPage.Should().Be(searchInput.PerPage);
			output.Total.Should().Be(quantityUserToGenerate);
			output.Items.Should().HaveCount(expectedQuantityItems);

			foreach (User outputItem in output.Items)
			{
				var exampleItem = exampleUserList.Find(
					category => category.Id == outputItem.Id
				);
				exampleItem.Should().NotBeNull();
				outputItem.Id.Should().Be(exampleItem!.Id);
				outputItem.BusinessName.Should().Be(exampleItem.BusinessName);
				outputItem.CorporateName.Should().Be(exampleItem.CorporateName);
				outputItem.Email.Should().Be(exampleItem.Email);
			}
		}

		[Theory(DisplayName = nameof(Search_By_Text))]
		[InlineData("joão", 1, 10, 1, 1)]
		[InlineData("João alfredo", 1, 10, 0, 0)]
		[InlineData("Lyra", 1, 10, 1, 1)]
		[InlineData("NetWork", 1, 10, 3, 3)]
		[InlineData("NetWork", 3, 1, 1, 3)]
		[InlineData("", 2, 1, 1, 4)]
		[InlineData("", 1, 2, 2, 4)]
		[Trait("Integration/Infra.Data", "UserRepository - Repositories")]
		public async Task Search_By_Text(
			string search,
			int page,
			int itemsPerPage,
			int expectedQuantityItemsReturned,
			int expectedQuantityTotalItems
		)
		{
			// Arrange
			var dbContext = _fixture.CreateDbContext();
			var exampleUsersList = _fixture.GetExampleListUsersByNames(new List<string[]> ()
			{
				new string[] { "João Silva", "Lyra", "Lyra Network " },
				new string[] { "Kendra Reichert", "Nubank", "Nubank Network" },
				new string[] { "Lucas Silveira", "Google", "Google Network" },
				new string[] { "Maria Silva", "Mercado Livre", "Mercado Livre LC" },
			});
			await dbContext.AddRangeAsync(exampleUsersList);
			await dbContext.SaveChangesAsync(CancellationToken.None);
			var categoryRepository = new Repository.UserRepository(dbContext);
			var searchInput = new SearchInput(page, itemsPerPage, search, "", SearchOrder.Asc);

			// Act
			var output = await categoryRepository.Search(searchInput, CancellationToken.None);

			// Assert
			output.Should().NotBeNull();
			output.Items.Should().NotBeNull();
			output.CurrentPage.Should().Be(searchInput.Page);
			output.PerPage.Should().Be(searchInput.PerPage);
			output.Total.Should().Be(expectedQuantityTotalItems);
			output.Items.Should().HaveCount(expectedQuantityItemsReturned);

			foreach (User outputItem in output.Items)
			{
				var exampleItem = exampleUsersList.Find(
					category => category.Id == outputItem.Id
				);
				exampleItem.Should().NotBeNull();
				outputItem.Id.Should().Be(exampleItem!.Id);
				outputItem.Name.Should().Be(exampleItem.Name);
				outputItem.BusinessName.Should().Be(exampleItem.BusinessName);
				outputItem.CorporateName.Should().Be(exampleItem.CorporateName);
				outputItem.Email.Should().Be(exampleItem.Email);
			}
		}

		[Theory(DisplayName = nameof(Search_Ordered))]
		[Trait("Integration/Infra.Data", "UserRepository - Repositories")]
		[InlineData("name", "asc")]
		[InlineData("name", "desc")]
		[InlineData("businessName", "asc")]
		[InlineData("businessName", "desc")]
		[InlineData("", "asc")]
		public async Task Search_Ordered(string orderBy, string order)
		{
			// Arrange
			var dbContext = _fixture.CreateDbContext();
			var exampleGenresList = _fixture.GetUserValidList(10);
			await dbContext.AddRangeAsync(exampleGenresList);
			await dbContext.SaveChangesAsync(CancellationToken.None);
			var repository = new Repository.UserRepository(dbContext);
			var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
			var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

			// Act
			var output = await repository.Search(searchInput, CancellationToken.None);

			// Assert
			var expectedOrderedList = _fixture.CloneUserListOrdered(exampleGenresList, orderBy, searchOrder);
			output.Should().NotBeNull();
			output.Items.Should().NotBeNull();
			output.CurrentPage.Should().Be(searchInput.Page);
			output.PerPage.Should().Be(searchInput.PerPage);
			output.Total.Should().Be(exampleGenresList.Count);
			output.Items.Should().HaveCount(exampleGenresList.Count);
			for (int indice = 0; indice < expectedOrderedList.Count; indice++)
			{
				var expectedItem = expectedOrderedList[indice];
				var outputItem = output.Items[indice];
				expectedItem.Should().NotBeNull();
				outputItem.Should().NotBeNull();
				outputItem.Name.Should().Be(expectedItem!.Name);
				outputItem.Id.Should().Be(expectedItem.Id);
				outputItem.BusinessName.Should().Be(expectedItem.BusinessName);
				outputItem.CorporateName.Should().Be(expectedItem.CorporateName);
				outputItem.Email.Should().Be(expectedItem.Email);
			}
		}
	}
}
