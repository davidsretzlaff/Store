using MediatR;
using Store.Application.UseCases.Delivery.Common;
using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.Delivery.CreateDelivery
{
	public record CreateDeliveryInput
	(
		string OrderId,
		string DeliveryType,
		AddressInput AddressCustomer,
		string? CompanyRegisterNumber
	) : IRequest<DeliveryOutput>;
}
