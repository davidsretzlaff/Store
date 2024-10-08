﻿using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.SeedWork;
using Store.Domain.Validation;
using Store.Domain.ValueObject;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Store.Domain.Entity
{
	public class Order : AggregateRoot
	{
		public string OrderId { get; private set; }
		public Cnpj CompanyIdentificationNumber { get; private set; }
        public DateTime CreatedDate { get; private set; }		
		public string CustomerName { get; private set; }
		public Cpf CustomerDocument {  get; private set; }
		public OrderStatus Status { get; private set; }
		[NotMapped]
		public List<Item> Items { get; private set; }

		public Order(string companyIdentificationNumber, string customerName, string customerDocument)
		{
			OrderId = GenerateOrderCode();
			CompanyIdentificationNumber = new Cnpj(companyIdentificationNumber);
			CreatedDate = DateTime.Now;
			CustomerName = customerName;
			CustomerDocument = new Cpf(customerDocument);
			Status = OrderStatus.Created;
			Items = new List<Item> { };
		}

		public Order() 
		{
			CompanyIdentificationNumber = new Cnpj();
			OrderId = GenerateOrderCode();
			Items = new List<Item>();
			CustomerName = string.Empty;
			CustomerDocument = new Cpf();
		}

		public int GetProductCount()
		{
			var count = 0;
			foreach (var item in Items)
			{
				count = count + item.Quantity;
			}
			return count;
		}

		public void AddItem(int productId, int quantity, Product? product = null)
		{
			var item = new Item(OrderId, productId, quantity);
			if (product is not null)
			{
				item.addProduct(product);
			}
			Items.Add(item);
			
		}

		public void Validate()
		{
			Items.ForEach(p => DomainValidation.MaxQuantity(p.Quantity, 3, $"Product with ID {p.ProductId} has a quantity of {p.Quantity}, but it"));
			DomainValidation.NotNullOrEmpty(CustomerName, nameof(CustomerName));
			DomainValidation.MaxLength(CustomerName, 100, nameof(CustomerName));
			DomainValidation.MinLength(CustomerName, 4, nameof(CustomerName));
		}

		private static string GenerateOrderCode()
		{
			var random = new Random();
			var randomDigits = random.Next(100000000, 1000000000); 
			return $"OR{randomDigits}";
		}

		public void Approve() 
		{
			Status = OrderStatus.Approved;
		}

		public void Cancel()
		{
			Status = OrderStatus.Canceled;
		}

		public string GetTotalAsCurrency()
		{
			decimal total = 0;
			total = Items.Sum(item => item.GetTotal());
			var money = new Money(total);
			return money.Format();
		}

		public string FormattedDate()
		{
			return CreatedDate.ToString("dd/MM/yyyy HH:mm:ss");
		}

		public bool IsApproved() 
		{ 
			return Status == OrderStatus.Approved;
		}
	}
}
