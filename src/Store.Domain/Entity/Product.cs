using Store.Domain.Enum;
using Store.Domain.SeedWork;

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

		public int GetQuantity()
		{
			return Quantity; 
		}
	}
}
