using MediatR;
using Store.Application.UseCases.Order.Common;
using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.Order.ApproveOrder
{
	public record class ApproveOrderInput(
		string Id,
		string? User
	) : IRequest<UpdateOrderOutput>
	{
	}
}
