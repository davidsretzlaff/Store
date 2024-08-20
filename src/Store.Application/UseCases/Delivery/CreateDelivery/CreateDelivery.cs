using Store.Application.Common.Exceptions;
using Store.Application.UseCases.Delivery.Common;
using Store.Domain.Extensions;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Delivery.CreateDelivery
{
	public class CreateDelivery : ICreateDelivery
	{
		private readonly IDeliveryRepository _deliveryRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;

		public CreateDelivery
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

		public async Task<DeliveryOutput> Handle(CreateDeliveryInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.User, cancellationToken);
			await ValidateInput(input, cancellationToken);
			var order = await _orderRepository.Get(input.OrderId, cancellationToken);
			var delivery = CreateDeliveryDomain(input, order);
			await _deliveryRepository.Insert(delivery, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return DeliveryOutput.FromDelivery(delivery);
		}

		private async Task ValidateInput(CreateDeliveryInput input, CancellationToken cancellationToken)
		{
			var existingDelivery = await _deliveryRepository.Get(input.OrderId, cancellationToken);
			DuplicateException.ThrowIfHasValue(existingDelivery, "A delivery already exists for this order.");
			DeliveryTypeInvalidException.ThrowIfInvalid(input.DeliveryType);
        }

		private DomainEntity.Delivery CreateDeliveryDomain(CreateDeliveryInput input, DomainEntity.Order? order)
		{
			return new DomainEntity.Delivery(
				input.OrderId,
				input.AddressCustomer.ToDomainAddress(),
				order,
				input.User!,
				input.DeliveryType.ToDeliveryTypeStatus()
			);
		}
	}
}
