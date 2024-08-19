using MediatR;
using Store.Application.UseCases.Delivery.Common;

namespace Store.Application.UseCases.Delivery.CompleteDelivery
{
	public interface ICompleteDelivery : IRequestHandler<CompleteDeliveryInput, DeliveryOutput>
	{
	}
}
