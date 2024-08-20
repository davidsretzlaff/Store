using Store.Application.Common.Exceptions;
using Store.Application.UseCases.Delivery.Common;
using Store.Application.UseCases.Order.ApproveOrder;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Delivery.CompleteDelivery
{
	public class CompleteDelivery : ICompleteDelivery
	{
		private readonly IDeliveryRepository _deliveryRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;

		public CompleteDelivery
		(
			IDeliveryRepository deliveryRepository,
			IOrderRepository orderRepository,
			IUnitOfWork unitOfWork,
			IUserValidation userValidation
		)
		{
			_deliveryRepository = deliveryRepository;
			_orderRepository = orderRepository;
			_unitOfWork = unitOfWork;
			_userValidation = userValidation;
		}

		public async Task<DeliveryOutput> Handle(CompleteDeliveryInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.Cnpj, cancellationToken);
			
			var delivery = await _deliveryRepository.Get(input.OrderId, cancellationToken);
			ValidateApproval(input,delivery);
			delivery!.CompleteDelivery();

			await _deliveryRepository.Update(delivery, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return DeliveryOutput.FromDelivery(delivery);
		}

		private void ValidateApproval(CompleteDeliveryInput input, DomainEntity.Delivery? delivery)
		{
			AggregateDomainException.ThrowIfNull(delivery, $"Delivery with ID {input.OrderId} not found");
			InvalidOrderOwnershipException.ThrowIfNotOwnership(delivery, input.Cnpj, $"Operation failed: The user is not the owner of this delivery");
		}
	}
}
