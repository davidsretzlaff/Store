using MediatR;
using Store.Application.UseCases.Delivery.Common;
namespace Store.Application.UseCases.Delivery.StartDelivery
{
	public record StartDeliveryInput
	(
		string OrderId,
		string? Cnpj
	): IRequest<DeliveryOutput>;
}
