using Store.Domain.Enum;
using Store.Domain.SeedWork;
using Store.Domain.Validation;
using Store.Domain.ValueObject;

namespace Store.Domain.Entity
{
	public class Product : AggregateRoot
	{
		public int ProductId { get; private set; }
		public string Title { get; private set; }
		public string Description { get; private set; }
		public decimal Price { get; private set; }
		public Category Category { get; private set; }

		public Product(int id, string title, string description, decimal price, Category category)
		{
			ProductId = id;
			Title = title;
			Description = description;
			Price = price;
			Category = category;
			Validate();
		}
		public Product()
		{
			ProductId = 0;
			Title = string.Empty;
			Description = string.Empty;
			Price = 0;
			Category = new Category();
		}

		private void Validate() 
		{
			DomainValidation.NotNullOrEmpty(Title, nameof(Title));
			DomainValidation.MinLength(Title, 4, nameof(Title));

			DomainValidation.NotNullOrEmpty(Description, nameof(Description));
			DomainValidation.MinLength(Description, 4, nameof(Description));

			DomainValidation.NotNull(ProductId, nameof(ProductId));
			DomainValidation.NotNull(Price, nameof(Price));
			DomainValidation.NotNull(Category, nameof(Category));
			DomainValidation.ValidateCategory(Category, nameof(Category));

		}

		public string GetPriceAsCurrency()
		{
			return FormatCurrency(Price);
		}

		private string FormatCurrency(decimal amount)
		{
			var money = new Money(amount);
			return money.Format();
		}
	}
}
