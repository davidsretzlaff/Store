using Store.Domain.Extensions;
using DomainEntity = Store.Domain.Entity;
namespace Store.Infra.Adapters.ExternalCatalog.Models
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }
		public string Image { get; set; }
		public RatingDto Rating { get; set; }

		public DomainEntity.Product ToProduct()
		{
			return new DomainEntity.Product(Id, Title, Description, Price, Category.ToCategory());
		}
	}
	public class RatingDto
	{
		public decimal Rate { get; set; }
		public int Count { get; set; }
	}


}
