using MediatR;

namespace Store.Application.UseCases.Order.ListOrders
{
	public interface IListOrders : IRequestHandler<ListOrdersInput, ListOrdersOutput>
	{
	}
}
