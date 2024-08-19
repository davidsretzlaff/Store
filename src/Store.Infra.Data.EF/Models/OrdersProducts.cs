namespace Store.Infra.Data.EF.Models
{
	public class OrdersProducts
	{
		public OrdersProducts(string orderId, int productId, int quantity)
		{
			OrderId = orderId;
			ProductId = productId;
			Quantity = quantity;
		}

		public string OrderId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get;set; }
	}
}
