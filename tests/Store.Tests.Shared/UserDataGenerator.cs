using Bogus;
using Bogus.Extensions.Brazil;
using Store.Application.UseCases.Auth.CreateAuth;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateUser;
using Store.Domain.Enum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Store.Tests.Shared
{
	public class UserDataGenerator : BaseFixture
	{
		public Domain.Entity.User GetValidUser()
		{

			return new Domain.Entity.User(
				Faker.Person.UserName,
				Faker.Hashids.ToString()!,
				Faker.Company.CompanyName(),
				Faker.Company.CompanyName(),
				Faker.Person.Email,
				Faker.Internet.Locale,
				"55 992364499",
				Faker.Company.Cnpj(),
				GetEmail()
			);
		}

		private Domain.ValueObject.Address GetEmail()
		{
			return new Domain.ValueObject.Address(
				Faker.Address.StreetName(),
				Faker.Address.City(),
				Faker.Address.State(),
				Faker.Address.Country(),
				Faker.Address.ZipCode()
			);
		}
		private Domain.Entity.User GetExampleUser(string[] inputs = null)
		{
			var user = GetValidUser();
			return new Domain.Entity.User(
				inputs[0],
				Faker.Hashids.ToString()!, 
				inputs[1], 
				inputs[2], 
				user.Email, 
				user.SiteUrl, 
				user.Phone, 
				user.CompanyRegistrationNumber, 
				user.Address
			);
		}

		public List<Domain.Entity.User> GetUserValidList(int quantity)
		{
			return Enumerable.Range(1, quantity).Select(_ => GetValidUser()).ToList();
		}

		public List<Domain.Entity.User> GetExampleListUsersByNames(List<string[]> inputs)
			=> inputs.Select(input => GetExampleUser(inputs: input)).ToList();

		public List<Domain.Entity.User> CloneUserListOrdered(List<Domain.Entity.User> userList, string orderBy, SearchOrder order)
		{
			var listClone = new List<Domain.Entity.User>(userList);
			var orderedEnumerable = (orderBy.ToLower(), order) switch
			{
				("username", SearchOrder.Asc) => listClone.OrderBy(x => x.UserName).ThenBy(x => x.Id),
				("username", SearchOrder.Desc) => listClone.OrderByDescending(x => x.UserName).ThenByDescending(x => x.Id),
				("businessname", SearchOrder.Asc) => listClone.OrderBy(x => x.BusinessName).ThenBy(x => x.Id),
				("bussinesname", SearchOrder.Desc) => listClone.OrderByDescending(x => x.BusinessName).ThenByDescending(x => x.Id),
				("corporatename", SearchOrder.Asc) => listClone.OrderBy(x => x.CorporateName).ThenBy(x => x.Id),
				("corporatename", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CorporateName).ThenByDescending(x => x.Id),
				("email", SearchOrder.Asc) => listClone.OrderBy(x => x.Email).ThenBy(x => x.Id),
				("email", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Email).ThenByDescending(x => x.Id),
				_ => listClone.OrderBy(x => x.BusinessName).ThenBy(x => x.Id)
			};
			return orderedEnumerable.ToList();
		}

		public List<Domain.Entity.User> GetExampleUserList(int listLength = 15)
		{
			var userList = Enumerable.Range(1, listLength).Select(
				_ => GetValidUser()
			).ToList();
			return userList;
		}

		public CreateUserInput getExampleInput()
		{
			var user = GetValidUser();
			return new(
				user.UserName,
				user.Password,
				user.BusinessName,
				user.CorporateName,
				user.Email,
				user.SiteUrl,
				user.Phone,
				user.CompanyRegistrationNumber,
				AddressInput.FromDomainAddress(user.Address)
			);
		}
		public CreateUserInput GetCreateUserInput()
		{
			var user = GetValidUser();
			return new(
				"1",
				user.UserName,
				user.BusinessName,
				user.CorporateName,
				user.Email,
				user.SiteUrl,
				user.Phone,
				user.CompanyRegistrationNumber,
				AddressInput.FromDomainAddress(user.Address)
			);
		}

		public CreateAuthInput GetCreateAuthInput(Domain.Entity.User user)
		{
			return new CreateAuthInput(user.UserName, user.Password);
		}
	}
}
