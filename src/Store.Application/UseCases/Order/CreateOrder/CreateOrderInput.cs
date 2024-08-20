using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Order.CreateOrder
{
	public record CreateOrderInput(
		string Cnpj,
		string CustomerName,
		string CustomerDocument,
		List<int> ProductIds
		) : IRequest<OrderOutput>;
}
