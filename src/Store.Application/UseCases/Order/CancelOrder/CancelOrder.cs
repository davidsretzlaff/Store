using Store.Application.Common.Exceptions;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;

namespace Store.Application.UseCases.Order.CancelOrder
{
    internal class CancelOrder : ICancelOrder
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _uservaValidation;

		public CancelOrder(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IUserValidation userValidation)
		{
			_unitOfWork = unitOfWork;
			_orderRepository = orderRepository;
			_uservaValidation = userValidation;
		}
		public async Task<UpdateOrderOutput> Handle(CancelOrderInput input, CancellationToken cancellationToken)
		{
			await _uservaValidation.IsUserActive(input.User, cancellationToken);
			var order = await _orderRepository.Get(input.Id, cancellationToken);
			
			AggregateDomainException.ThrowIfNull(order, $"Order with ID {input.Id} not found");
			InvalidOrderOwnershipException.ThrowIfNotOwnership(order, input.User, $"Operation failed: The user is not the owner of this order");
			
			order!.Cancel();
			await _orderRepository.Update(order, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return UpdateOrderOutput.FromOrder(order);
		}
	}
}
