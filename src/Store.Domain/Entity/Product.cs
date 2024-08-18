using Store.Domain.Enum;
using Store.Domain.SeedWork;
using Store.Domain.ValueObject;

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
