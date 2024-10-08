﻿
using Store.Domain.Entity;
using Store.Domain.Interface.Infra.Adapters;
using Store.Infra.Adapters.CacheService.Models;

namespace Store.Infra.Adapters.CacheService
{
    public class CacheService : ICacheService
	{
		private readonly Dictionary<int, CachedProduct> _productCache = new Dictionary<int, CachedProduct>();
		private bool _allProductsCached = false;
		private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(10);

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
			if (product == null)
			{
				throw new ArgumentNullException(nameof(product));
			}
			_productCache[product.ProductId] = new CachedProduct(new ProductModel(product), DateTime.UtcNow);
			RemoveProductFromDeleted(product.ProductId);
		}
		public void RemoveProductFromDeleted(int productId)
		{
			if (IsProductDeleted(productId))
			{
				DeletedProducts.Remove(productId);
			}
		}
		public void RemoveProductFromCache(int productId)
		{
			_productCache.Remove(productId);
		}

		public Product? GetCachedProduct(int id)
		{
			if (_productCache.TryGetValue(id, out var cachedProduct) && !cachedProduct.IsExpired(_cacheExpiration))
			{
				return cachedProduct.Product.ToProduct();
			}
			return null;
		}

		public IEnumerable<Product> GetAllCachedProducts()
		{
			return _productCache.Values
				.Where(cp => cp != null && !cp.IsExpired(_cacheExpiration))
				.Select(cp => cp.Product.ToProduct());
		}

		public void MarkAllProductsAsCached()
		{
			_allProductsCached = true;
		}
	}
}
