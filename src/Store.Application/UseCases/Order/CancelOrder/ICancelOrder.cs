using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Order.CancelOrder
{
	public interface ICancelOrder :  IRequestHandler<CancelOrderInput, OrderOutput>
	{
	}
}
