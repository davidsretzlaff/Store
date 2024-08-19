using MediatR;

namespace Store.Application.UseCases.Delivery.ListDeliveries
{
	public interface IListDeliveries : IRequestHandler<ListDeliveriesInput, ListDeliveriesOutput>
	{
	}
}
