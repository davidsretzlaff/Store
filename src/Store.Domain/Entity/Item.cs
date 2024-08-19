using Store.Domain.ValueObject;

namespace Store.Domain.Entity
{
	public class Item
	{
		public string OrderId { get; private set; }
		public int ProductId { get; private set; }
		public decimal Price { get; private set; }
		public int Quantity { get; private set; }

		public Item(string orderId, int productId, decimal price)
		{
			OrderId = orderId;
			ProductId = productId;
			Price = price;
			Quantity = 0;
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
		public void AddOneToQuantity()
		{
			Quantity += 1;
		}
	}
}
