using Store.Application.Common.Exceptions;
using Store.Application.UseCases.Delivery.Common;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Delivery.StartDelivery
{
	public class StartDelivery : IStartDelivery
	{
		private readonly IDeliveryRepository _deliveryRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;

		public StartDelivery
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
		public async Task<DeliveryOutput> Handle(StartDeliveryInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.Cnpj, cancellationToken);

			var delivery = await _deliveryRepository.Get(input.OrderId, cancellationToken);
			ValidateDeliveryStart(input, delivery);
			delivery!.StartDelivery();

			await _deliveryRepository.Update(delivery, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return DeliveryOutput.FromDelivery(delivery);
		}

		private void ValidateDeliveryStart(StartDeliveryInput input, DomainEntity.Delivery? delivery)
		{
			AggregateDomainException.ThrowIfNull(delivery, $"Delivery with ID {input.OrderId} not found.");
			InvalidOrderOwnershipException.ThrowIfNotOwnership(delivery, input.Cnpj, $"Operation failed: The user is not the owner of this delivery");
		}
	}
}
