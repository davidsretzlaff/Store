using Store.Domain.Enum;
using Store.Domain.SeedWork;
using Store.Domain.Validation;
using Store.Domain.ValueObject;

namespace Store.Domain.Entity
{
	public class Delivery : AggregateRoot
	{
		public string OrderId { get; private set; }
		public DateTime DeliveredDate { get; private set; }
		public Address Address { get; private set; }
		public DeliveryStatus Status { get; private set; }
		public string CompanyRegisterNumber { get; private set; }

		private Order Order;

		public Delivery(string orderId, Address adddress, Order? order)
		{
			OrderId = orderId;
			DeliveredDate = new DateTime();
			Address = adddress;
			Status = DeliveryStatus.Pending;
			AddOrder(order);
			Validate();
		}
		private void AddOrder(Order? order) 
		{
			DomainValidation.NotNull(order, nameof(order));
			DomainValidation.NotFound(order, $"Order with id {order!.Id}");
			order.Validate();
			Order = order;
			CompanyRegisterNumber = order.CompanyRegisterNumber;
		}
		private void Validate() 
		{
			DomainValidation.NotNullOrEmpty(OrderId, nameof(OrderId));
			DomainValidation.NotNullOrEmpty(CompanyRegisterNumber, nameof(CompanyRegisterNumber));
		}

		public void StartDelivery()
		{
			DomainValidation.NotNull(Order, nameof(Order));
			DomainValidation.OrderIsNotApprove(Order);
			DomainValidation.DeliveryIsPending(Status);
			Status = DeliveryStatus.InTransit;
			
		}

		public void CompleteDelivery()
		{
			DomainValidation.NotNull(Order, nameof(Order));
			DomainValidation.OrderIsNotApprove(Order);
			DomainValidation.DeliveryIsInTransit(Status);
			Status = DeliveryStatus.Delivered;
			DeliveredDate = DateTime.UtcNow;
		}

		public string FormattedDeliveredDate()
		{
			return DeliveredDate.ToString("dd/MM/yyyy HH:mm:ss");
		}
	}
}
