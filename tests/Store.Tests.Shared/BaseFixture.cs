using Bogus;
using Microsoft.EntityFrameworkCore;
using Store.Infra.Data.EF;

namespace Store.Tests.Shared
{
	public abstract class BaseFixture
	{
		public Faker Faker { get; set; }

		protected BaseFixture() => Faker = new Faker("pt_BR");

		public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
	}
}
