
using Store.Domain.Enum;

namespace Store.Domain.Extensions
{
	public static class OrderStatusExtensions
	{
		public static OrderStatus ToOrderStatus(this string status)
		=> status switch
		{
			"Canceled" => OrderStatus.Canceled,
			"Approved" => OrderStatus.Approved,
			"Created" => OrderStatus.Created,
			_ => throw new ArgumentOutOfRangeException(nameof(status))
		};

		public static string ToOrderStringStatus(this OrderStatus status)
		=> status switch
		{
			OrderStatus.Canceled => "Canceled",
			OrderStatus.Approved => "Approved",
			OrderStatus.Created => "Created",
		};
	}
}
