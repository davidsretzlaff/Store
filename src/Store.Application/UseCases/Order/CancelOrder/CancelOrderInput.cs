using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Order.CancelOrder
{
	public record class CancelOrderInput(
		string id,
		string CompanyRegisterNumber
	) : IRequest<OrderOutput>
	{
	}
}
