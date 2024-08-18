﻿using Store.Domain.Enum;
using Store.Domain.SeedWork;
using Store.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Store.Domain.Entity
{
	public class Order : AggregateRoot
	{	
		public string Id { get; private set; }
		public string CompanyRegisterNumber { get; private set; }
        public DateTime CreatedData { get; private set; }		
		public string CustomerName { get; private set; }
		public string CustomerDocument {  get; private set; }
		public OrderStatus Status { get; private set; }
		[NotMapped]
		public List<Product> Products { get; private set; }

		public Order(string companyRegisterNumber, string customerName, string customerDocument)
		{
			Id = GenerateOrderCode();
			CompanyRegisterNumber = companyRegisterNumber;
			CreatedData = DateTime.Now;
			CustomerName = customerName;
			CustomerDocument = customerDocument;
			Status = OrderStatus.Created;
			Products = new List<Product> { };
		}

		public int GetTotalProducts()
		{
			return Products.Count; 
		}
		public void AddProduct(Product product)
		{
			
			var index = Products.FindIndex(p => p.Id == product.Id);

			if (index >= 0)
			{
				Products[index].AddOneToQuantity();
				return;
			}

			Products.Add(product);
		}
		public void Validate() 
		{
			Products.ForEach(p => DomainValidation.MaxQuantity(p.Quantity, 3, nameof(p.Title)));
			DomainValidation.NotNullOrEmpty(CompanyRegisterNumber, nameof(CompanyRegisterNumber));
			DomainValidation.NotNullOrEmpty(CustomerName, nameof(CustomerName));
			DomainValidation.MaxLength(CustomerName, 100, nameof(CustomerName));
			DomainValidation.MinLength(CustomerName, 4, nameof(CustomerName));

			DomainValidation.NotNullOrEmpty(CustomerDocument, nameof(CustomerDocument));
			//david melhorar criar validador CPF
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

		public string GetTotal()
		{
			decimal total = 0;
			total = Products.Sum(item => item.GetTotal());
			return FormatAsCurrency(total);
		}
		public string FormatAsCurrency(decimal amount)
		{
			var cultureInfo = new CultureInfo("pt-BR"); // Brazilian Portuguese
			cultureInfo.NumberFormat.CurrencySymbol = "R$";
			cultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";
			cultureInfo.NumberFormat.CurrencyGroupSeparator = ".";
			cultureInfo.NumberFormat.CurrencyGroupSizes = new[] { 3 };

			return amount.ToString("C", cultureInfo);
		}
	}
}
