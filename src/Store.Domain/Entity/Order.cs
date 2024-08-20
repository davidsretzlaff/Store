using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.SeedWork;
using Store.Domain.Validation;
using Store.Domain.ValueObject;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Store.Domain.Entity
{
	public class Order : AggregateRoot
	{	
		public string Id { get; private set; }
		public CNPJ Cnpj { get; private set; }
        public DateTime CreatedDate { get; private set; }		
		public string CustomerName { get; private set; }
		public string CustomerDocument {  get; private set; }
		public OrderStatus Status { get; private set; }
		[NotMapped]
		public List<Item> Items { get; private set; }

		public Order(string cnpj, string customerName, string customerDocument)
		{
			Id = GenerateOrderCode();
			Cnpj = new CNPJ(cnpj);
			CreatedDate = DateTime.Now;
			CustomerName = customerName;
			CustomerDocument = customerDocument;
			Status = OrderStatus.Created;
			Items = new List<Item> { };
		}
		public Order() 
		{
			Items = new List<Item>();
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
			var item = new Item(Id, productId, quantity);
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
