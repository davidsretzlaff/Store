
using Bogus;
using Bogus.Extensions.Brazil;
using Store.Application.UseCases.Order.CreateOrder;

namespace Store.Tests.Shared
{
	public class OrderDataGenerator : BaseFixture
	{
		public CreateOrderInput CreateOrderInput()
		{
			var input = new CreateOrderInput(
				Faker.Company.Cnpj(),
				Faker.Person.FullName,
				Faker.Person.Cpf(),
				new List<int> { 6,7,8}
			);
			return input;
		}
	}
}
