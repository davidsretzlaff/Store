﻿using Bogus;
using Bogus.Extensions.Brazil;
using Store.Domain.Enum;

namespace Store.Tests.Shared
{
	public class UserDataGenerator : BaseFixture
	{
		public Domain.Entity.User GetValidUser()
		{

			return new Domain.Entity.User(
				Faker.Person.FullName,
				Faker.Company.CompanyName(),
				Faker.Company.CompanyName(),
				Faker.Person.Email,
				"www.sitecompany.com.br",
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
			return new Domain.Entity.User(inputs[0], inputs[1], inputs[2], user.Email, user.SiteUrl, user.Phone, user.CompanyRegistrationNumber, user.Address);
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
				("Name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
				("Name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
				("BusinessName", SearchOrder.Asc) => listClone.OrderBy(x => x.BusinessName).ThenBy(x => x.Id),
				("BusinessName", SearchOrder.Desc) => listClone.OrderByDescending(x => x.BusinessName).ThenByDescending(x => x.Id),
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
		
	}
}
