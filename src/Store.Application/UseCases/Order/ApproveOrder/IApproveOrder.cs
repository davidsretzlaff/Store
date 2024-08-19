using MediatR;
using Store.Application.UseCases.Order.Common;
using Store.Application.UseCases.User.ActivateUser;

namespace Store.Application.UseCases.Order.ApproveOrder
{
	public interface IApproveOrder : IRequestHandler<ApproveOrderInput, UpdateOrderOutput>
	{
	}
}
