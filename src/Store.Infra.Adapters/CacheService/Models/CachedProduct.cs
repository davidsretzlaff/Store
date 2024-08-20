
using Store.Domain.Entity;
using Store.Domain.Enum;

namespace Store.Infra.Adapters.CacheService.Models
{
	public class ProductModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public Category Category { get; set; }

		public ProductModel(Product product)
		{
			Id = product.ProductId;
			Title = product.Title;
			Description = product.Description;
			Price = product.Price;
			Category = product.Category;
		}

		public Product ToProduct()
		{
			return new Product(Id, Title, Description, Price, Category);
		}
	}

	public class CachedProduct
	{
		public ProductModel Product { get; }
		public DateTime Timestamp { get; }

		public CachedProduct(ProductModel product, DateTime timestamp)
		{
			Product = product;
			Timestamp = timestamp;
		}

		public bool IsExpired(TimeSpan cacheExpiration) =>
			(DateTime.UtcNow - Timestamp) >= cacheExpiration;
	}
}
