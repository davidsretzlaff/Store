using Store.Application.UseCases.User.Common;
using Store.Domain.Extensions;
using DomainEntity = Store.Domain.Entity;
namespace Store.Application.UseCases.Delivery.Common
{
	public record DeliveryOutput
	(
		string OrderId,
		string DeliveredDate,
		AddressOutput Adddress,	
		string Status
	)
	{
		public static DeliveryOutput FromDelivery(DomainEntity.Delivery delivery)
		{
			return new DeliveryOutput(
				delivery.OrderId,
				delivery.FormattedDeliveredDate(),
				   new AddressOutput(
					   delivery.Address.Street,
					   delivery.Address.City,
					   delivery.Address.State,
					   delivery.Address.Country,
					   delivery.Address.ZipCode
					),
				   delivery.Status.ToDeliveryStatusString()
			   );
		}
	}
}
