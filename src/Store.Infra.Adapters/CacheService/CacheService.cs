
using Store.Domain.Entity;
using Store.Domain.Interface;

namespace Store.Infra.Adapters.CacheService
{
	public class CacheService : ICacheService
	{
		private readonly Dictionary<int, CachedProduct> _productCache = new Dictionary<int, CachedProduct>();
		private bool _allProductsCached = false;
		private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(3);

		public HashSet<int> DeletedProducts { get; } = new HashSet<int>();
		public bool AllProductsCached => _allProductsCached;


		public bool IsProductDeleted(int id)
		{
			return DeletedProducts.Contains(id);
		}
		public void MarkProductAsDeleted(int id)
		{
			if (!IsProductDeleted(id))
			{
				DeletedProducts.Add(id);
			}
		}
		public void CacheProduct(Product product)
		{
			if (product == null) throw new ArgumentNullException(nameof(product));
			_productCache[product.Id] = new CachedProduct(product, DateTime.UtcNow);
		}

		public void RemoveProductFromCache(int productId)
		{
			_productCache.Remove(productId);
		}

		public Product? GetCachedProduct(int id)
		{
			if (_productCache.TryGetValue(id, out var cachedProduct) && !cachedProduct.IsExpired(_cacheExpiration))
			{
				return cachedProduct.Product;
			}
			return null;
		}

		public IEnumerable<Product> GetAllCachedProducts()
		{
			return _productCache.Values
				.Where(cp => !cp.IsExpired(_cacheExpiration))
				.Select(cp => cp.Product);
		}

		public void MarkAllProductsAsCached()
		{
			_allProductsCached = true;
		}

		private class CachedProduct
		{
			public Product Product { get; }
			public DateTime Timestamp { get; }

			public CachedProduct(Product product, DateTime timestamp)
			{
				Product = product ?? throw new ArgumentNullException(nameof(product));
				Timestamp = timestamp;
			}

			public bool IsExpired(TimeSpan cacheExpiration) =>
				(DateTime.UtcNow - Timestamp) >= cacheExpiration;
		}
	}
}
