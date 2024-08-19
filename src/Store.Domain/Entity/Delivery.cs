using Store.Domain.Enum;
using Store.Domain.SeedWork;
using Store.Domain.Validation;
using Store.Domain.ValueObject;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entity
{
	public class Delivery : AggregateRoot
	{
		public string OrderId { get; private set; }
		public DateTime DeliveredDate { get; private set; }
		public Address Address { get; private set; }
		public DeliveryStatus Status { get; private set; }
		public string CompanyRegisterNumber { get; private set; }
		[NotMapped]
		public Order Order { get; private set; }

		public Delivery(string orderId, Address adddress, Order? order)
		{
			OrderId = orderId;
			DeliveredDate = new DateTime();
			Address = adddress;
			Status = DeliveryStatus.Pending;
			AddOrder(order);
			Validate();
		}
		public Delivery() { }
		private void AddOrder(Order? order) 
		{
			DomainValidation.NotFound(order, $"Order with id {OrderId}");
			order.Validate();
			Order = order;
			CompanyRegisterNumber = order.CompanyRegisterNumber;
		}
		private void Validate() 
		{
			DomainValidation.OrderIsNotApprove(Order);
			DomainValidation.NotNullOrEmpty(OrderId, nameof(OrderId));
			DomainValidation.NotNullOrEmpty(CompanyRegisterNumber, nameof(CompanyRegisterNumber));
		}

		public void StartDelivery()
		{
			DomainValidation.DeliveryIsPending(Status);
			Status = DeliveryStatus.InTransit;
		}

		public void CompleteDelivery()
		{
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
