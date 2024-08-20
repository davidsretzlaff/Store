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
		public DeliveryType DeliveryType { get; private set; }
		public Cnpj CompanyIdentificationNumber { get; private set; }
		[NotMapped]
		public Order Order { get; private set; }

		public Delivery(string orderId, Address adddress, Order? order, string companyIdentificationNumber, DeliveryType deliveryType)
		{
			OrderId = orderId;
			DeliveredDate = new DateTime();
			Address = adddress;
			Status = DeliveryStatus.Pending;
			CompanyIdentificationNumber = new Cnpj(companyIdentificationNumber);
			Order = new Order();
			AddOrder(order);
			Validate();
			DeliveryType = deliveryType;
		}
		public Delivery() 
		{
			OrderId = string.Empty;
			DeliveredDate = new DateTime();
			Address = new Address();
			Status = DeliveryStatus.Pending;
			CompanyIdentificationNumber = new Cnpj();
			Order = new Order();
		}
		private void AddOrder(Order? order) 
		{
			DomainValidation.NotFound(order, $"Order with id {OrderId}");
			order!.Validate();
			Order = order;
			CompanyIdentificationNumber = order.CompanyIdentificationNumber;
		}
		private void Validate() 
		{
			DomainValidation.OrderIsNotApprove(Order);
			DomainValidation.NotNullOrEmpty(OrderId, nameof(OrderId));
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
