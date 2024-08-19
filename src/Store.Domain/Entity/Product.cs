using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.SeedWork;
using Store.Domain.Validation;
using Store.Domain.ValueObject;
using System.Net.Http.Headers;

namespace Store.Domain.Entity
{
	public class Product : AggregateRoot
	{
		public int Id { get; private set; }
		public string Title { get; private set; }
		public string Description { get; private set; }
		public decimal Price { get; private set; }
		public Category Category { get; private set; }
		public int Quantity { get; private set; }

		public Product(int id, string title, string description, decimal price, Category category)
		{
			Id = id;
			Title = title;
			Description = description;
			Price = price;
			Category = category;
			Quantity = 1;
			Validate();
		}

		public Product(int id)
		{ 
			Id = id;
		}

			private void Validate() 
		{
			DomainValidation.NotNullOrEmpty(Title, nameof(Title));
			DomainValidation.MinLength(Title, 4, nameof(Title));

			DomainValidation.NotNullOrEmpty(Description, nameof(Description));
			DomainValidation.MinLength(Description, 4, nameof(Description));

			DomainValidation.NotNull(Id, nameof(Id));
			DomainValidation.NotNull(Price, nameof(Price));
			DomainValidation.NotNull(Category, nameof(Category));
			DomainValidation.ValidateCategory(Category, nameof(Category));

		}
		public void AddOneToQuantity() 
		{
			Quantity += 1;
		}
		public decimal GetTotal()
		{
			return Price * Quantity;
		}
		public string GetPriceAsCurrency()
		{
			return FormatCurrency(Price);
		}
		public string GetTotalAsCurrency()
		{
			return FormatCurrency(GetTotal());
		}

		private string FormatCurrency(decimal amount)
		{
			var money = new Money(amount);
			return money.Format();
		}
		public int GetQuantity()
		{
			return Quantity; 
		}
	}
}
