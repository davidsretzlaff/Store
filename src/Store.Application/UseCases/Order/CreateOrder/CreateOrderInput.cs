using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Order.CreateOrder
{
	public record CreateOrderInput(
		string? User,
		string CustomerName,
		string CustomerDocument,
		List<int> ProductIds
		) : IRequest<OrderOutput>;
}
