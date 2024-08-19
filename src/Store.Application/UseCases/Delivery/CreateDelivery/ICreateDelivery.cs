using MediatR;
using Store.Application.UseCases.Delivery.Common;

namespace Store.Application.UseCases.Delivery.CreateDelivery
{
	public interface ICreateDelivery : IRequestHandler<CreateDeliveryInput, DeliveryOutput>
	{
	}
}
