using Store.Domain.Enum;

namespace Store.Domain.Extensions
{
	public static class DeliveryTypeExtension
	{
		public static DeliveryType ToDeliveryTypeStatus(this string category)
		=> category.ToLower() switch
		{
			"javalog" => DeliveryType.Javalog,
			"csharplog" => DeliveryType.Csharplog,
			_ => throw new ArgumentOutOfRangeException(nameof(category))
		};

		public static string ToDeliveryTypeString(this DeliveryType category)
		=> category switch
		{
			DeliveryType.Javalog => "Javalog",
			DeliveryType.Csharplog => "Csharplog",
			_ => throw new ArgumentOutOfRangeException(nameof(category))
		};
	}
}
