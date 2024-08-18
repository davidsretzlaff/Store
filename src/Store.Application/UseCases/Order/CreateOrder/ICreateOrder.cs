using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Order.CreateOrder
{
	public interface ICreateOrder : IRequestHandler<CreateOrderInput, OrderOutput>
	{
	}
}
