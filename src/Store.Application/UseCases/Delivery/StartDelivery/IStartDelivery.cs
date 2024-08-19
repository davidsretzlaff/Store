using MediatR;
using Store.Application.UseCases.Delivery.Common;

namespace Store.Application.UseCases.Delivery.StartDelivery
{
	public interface IStartDelivery : IRequestHandler<StartDeliveryInput, DeliveryOutput>
	{
	}
}
