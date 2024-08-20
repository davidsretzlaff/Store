
using Store.Application.Common.Exceptions;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.ApproveOrder
{
    internal class ApproveOrder : IApproveOrder
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;

		public ApproveOrder(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IUserValidation userValidation)
		{
			_unitOfWork = unitOfWork;
			_orderRepository = orderRepository;
			_userValidation = userValidation;
		}
		public async Task<UpdateOrderOutput> Handle(ApproveOrderInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.User, cancellationToken);
			var order = await _orderRepository.Get(input.Id, cancellationToken);
			ValidateApproval(input, order);
			
			order!.Approve();
			await _orderRepository.Update(order, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return UpdateOrderOutput.FromOrder(order);
		}

		private void ValidateApproval(ApproveOrderInput input, DomainEntity.Order? order)
		{
			AggregateDomainException.ThrowIfNull(order, $"Order with ID {input.Id} not found");
			InvalidOrderOwnershipException.ThrowIfNotOwnership(order, input.User, $"Operation failed: The user is not the owner of this order");
		}
	}
}
