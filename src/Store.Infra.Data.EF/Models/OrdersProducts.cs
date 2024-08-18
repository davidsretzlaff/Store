namespace Store.Infra.Data.EF.Models
{
	public class OrdersProducts
	{
		public OrdersProducts(string orderId, int productId)
		{
			OrderId = orderId;
			ProductId = productId;
		}

		public string OrderId { get; set; }
		public int ProductId { get; set; }

	}
}
