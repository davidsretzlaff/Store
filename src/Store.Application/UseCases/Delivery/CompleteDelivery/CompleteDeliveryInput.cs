using MediatR;
using Store.Application.UseCases.Delivery.Common;

namespace Store.Application.UseCases.Delivery.CompleteDelivery
{
	public record CompleteDeliveryInput(
		string OrderId, 
		string CompanyRegisterNumber
	) : IRequest<DeliveryOutput>;
}
