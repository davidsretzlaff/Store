using Store.Domain.ValueObject;

namespace Store.Domain.Entity
{
	public class Item
	{
		public string OrderId { get; private set; }
		public int ProductId { get; private set; }
		public int Quantity { get; private set; }
		public Product Product { get; private set; }

		public Item(string orderId, int productId, int quantity)
		{
			OrderId = orderId;
			ProductId = productId;
			Quantity = quantity;
			
		}

		public void addProduct(Product product) 
		{
			Product = new Product(product.Id, product.Title, product.Description, product.Price, product.Category);
		}

		public decimal GetTotal()
		{
			return Product.Price * Quantity;
		}
		public string GetPriceAsCurrency()
		{
			return FormatCurrency(Product.Price);
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
		public void AddOneToQuantity()
		{
			Quantity += 1;
		}
	}
}
