
using Store.Domain.Enum;
using Store.Domain.Extensions;

namespace Store.Application.Common.Exceptions
{
	public class DeliveryTypeInvalidException : ApplicationException
	{
		public DeliveryTypeInvalidException(string message) : base(message)
		{ }

		public static void ThrowIfInvalid(string inputDeliveryType)
		{
			try
			{
				inputDeliveryType.ToDeliveryTypeStatus();
			}
			catch (Exception)
			{
				throw new DeliveryTypeInvalidException($"Invalid DeliveryType. " +
					$"Only {DeliveryType.Javalog.ToDeliveryTypeString()} " +
					$"and {DeliveryType.Csharplog.ToDeliveryTypeString()} are accepted");
			}
		}
	}
}
