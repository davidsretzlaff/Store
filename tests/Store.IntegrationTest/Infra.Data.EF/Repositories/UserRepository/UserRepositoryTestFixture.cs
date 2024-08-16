using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Tests.Shared;
using static Bogus.DataSets.Name;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Store.IntegrationTest.Infra.Data.EF.Repositories.UserRepository
{
	[CollectionDefinition(nameof(UserRepositoryTestFixture))]
	public class UserRepositoryTestFixtureCollection
	: ICollectionFixture<UserRepositoryTestFixture>
	{ }


	public class UserRepositoryTestFixture : BaseFixture
	{
		public UserDataGenerator DataGenerator { get; }
		public UserRepositoryTestFixture() => DataGenerator = new UserDataGenerator();
		public Domain.Entity.User GetValidUser() => DataGenerator.GetValidUser();
		public List<Domain.Entity.User> GetUserValidList(int quantity) => DataGenerator.GetUserValidList(quantity);

		private Domain.Entity.User GetExampleUser(string[] inputs = null)
		{
			var user = GetValidUser();
			return new Domain.Entity.User(inputs[0], inputs[1], inputs[2],user.Email,user.SiteUrl,user.Phone,user.CompanyRegistrationNumber,user.Address);
		}
		public List<Domain.Entity.User> GetExampleListUsersByNames(List<string[]> inputs)
			=> inputs.Select(input => GetExampleUser(inputs: input)).ToList();

		public List<Domain.Entity.User> CloneUserListOrdered(List<Domain.Entity.User> userList, string orderBy, SearchOrder order)
		{
			var listClone = new List<Domain.Entity.User>(userList);
			var orderedEnumerable = (orderBy.ToLower(), order) switch
			{
				("Name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
				("Name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
				("BusinessName", SearchOrder.Asc) => listClone.OrderBy(x => x.BusinessName).ThenBy(x => x.Id),
				("BusinessName", SearchOrder.Desc) => listClone.OrderByDescending(x => x.BusinessName).ThenByDescending(x => x.Id),
				_ => listClone.OrderBy(x => x.BusinessName).ThenBy(x => x.Id)
			};
			return orderedEnumerable.ToList();
		}
	}
}
