using Store.Application.UseCases.Delivery.CreateDelivery;
using Store.Application.UseCases.User.Common;

namespace Store.Api.Models.CreateDelivery
{
	public record ApiCreateDeliveryInput 
	(
		string OrderId,
		string DeliveryType,
		AddressInput AddressCustomer
	)
	{
		public CreateDeliveryInput ToInput(string? userDocument) 
		{
			return new CreateDeliveryInput(OrderId, DeliveryType, AddressCustomer, userDocument);
		}
	}
}
