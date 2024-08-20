
using Store.Domain.Enum;

namespace Store.Domain.Extensions
{
	public static class DeliveryStatusExtension
	{
		public static DeliveryStatus ToDeliveryStatus(this string category)
		=> category.ToLower() switch
		{
			"pending" => DeliveryStatus.Pending,
			"intransit" => DeliveryStatus.InTransit,
			"delivered" => DeliveryStatus.Delivered,
			_ => DeliveryStatus.Error
		};

		public static string ToDeliveryStatusString(this DeliveryStatus category)
		=> category switch
		{
			DeliveryStatus.Pending => "Pending",
			DeliveryStatus.InTransit => "InTransit",
			DeliveryStatus.Delivered => "Delivered",
			_ => throw new ArgumentOutOfRangeException(nameof(category))
		};
	}
}
