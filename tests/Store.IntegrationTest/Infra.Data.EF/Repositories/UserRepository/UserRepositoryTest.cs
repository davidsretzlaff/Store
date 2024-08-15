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
			var dbContext = _fixture.CreateDbContext();
			var exampleUserList = _fixture.GetUserValidList(15);
			await dbContext.AddRangeAsync(exampleUserList);
			await dbContext.SaveChangesAsync(CancellationToken.None);
			var userRepository = new Repository.UserRepository(dbContext);
			var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

			var output = await userRepository.Search(searchInput, CancellationToken.None);

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
			var dbContext = _fixture.CreateDbContext();
			var userRepository = new Repository.UserRepository(dbContext);
			var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

			var output = await userRepository.Search(searchInput, CancellationToken.None);

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
			var dbContext = _fixture.CreateDbContext();
			var exampleUserList = _fixture.GetUserValidList(quantityUserToGenerate);
			await dbContext.AddRangeAsync(exampleUserList);
			await dbContext.SaveChangesAsync(CancellationToken.None);
			var userRepository = new Repository.UserRepository(dbContext);
			var searchInput = new SearchInput(NumberOfPagesToReturn, ItemsPerPage, "", "", SearchOrder.Asc);

			var output = await userRepository.Search(searchInput, CancellationToken.None);

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
				outputItem.Id.Should().Be(exampleItem!.Id);
				outputItem.BusinessName.Should().Be(exampleItem.BusinessName);
				outputItem.CorporateName.Should().Be(exampleItem.CorporateName);
				outputItem.Email.Should().Be(exampleItem.Email);
			}
		}

		//[Theory(DisplayName = nameof(SearchByText))]
		//[InlineData("Action", 1, 5, 1, 1)]
		//[InlineData("Horror", 1, 5, 3, 3)]
		//[InlineData("Horror", 2, 5, 0, 3)]
		//[InlineData("Sci-fi", 1, 5, 4, 4)]
		//[InlineData("Sci-fi", 1, 2, 2, 4)]
		//[InlineData("Sci-fi", 2, 3, 1, 4)]
		//[InlineData("Sci-fi Other", 1, 5, 0, 0)]
		//[InlineData("Robots", 1, 5, 2, 2)]
		//[Trait("Integration/Infra.Data", "UserRepository - Repositories")]
		//public async Task Search_By_Text(
		//	string search,
		//	int NumberOfPagesToReturn,
		//	int ItemsPerPage,
		//	int expectedQuantityItemsReturned,
		//	int expectedQuantityTotalItems
		//)
		//{
		//	CatalogDbContext dbContext = _fixture.CreateDbContext();
		//	var exampleCategoriesList = _fixture.GetExampleCategoriesListWithNames(new List<string>()
		//	{
		//		"Action",
		//		"Horror",
		//		"Horror - Robots",
		//		"Horror - Based on Real Facts",
		//		"Drama",
		//		"Sci-fi IA",
		//		"Sci-fi Space",
		//		"Sci-fi Robots",
		//		"Sci-fi Future"
		//	});
		//	await dbContext.AddRangeAsync(exampleCategoriesList);
		//	await dbContext.SaveChangesAsync(CancellationToken.None);
		//	var categoryRepository = new Repository.CategoryRepository(dbContext);
		//	var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

		//	var output = await categoryRepository.Search(searchInput, CancellationToken.None);

		//	output.Should().NotBeNull();
		//	output.Items.Should().NotBeNull();
		//	output.CurrentPage.Should().Be(searchInput.Page);
		//	output.PerPage.Should().Be(searchInput.PerPage);
		//	output.Total.Should().Be(expectedQuantityTotalItems);
		//	output.Items.Should().HaveCount(expectedQuantityItemsReturned);

		//	foreach (Category outputItem in output.Items)
		//	{
		//		var exampleItem = exampleCategoriesList.Find(
		//			category => category.Id == outputItem.Id
		//		);
		//		exampleItem.Should().NotBeNull();
		//		outputItem.Id.Should().Be(exampleItem!.Id);
		//		outputItem.Name.Should().Be(exampleItem.Name);
		//		outputItem.Description.Should().Be(exampleItem.Description);
		//		outputItem.IsActive.Should().Be(exampleItem.IsActive);
		//		outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
		//	}
		//}
	}
}
