using Store.Domain.Entity;

namespace Store.Domain.Interface
{
	public interface ICacheService
	{
		bool AllProductsCached { get; }
		void CacheProduct(Product product);
		void RemoveProductFromCache(int productId);
		Product? GetCachedProduct(int id);
		IEnumerable<Product> GetAllCachedProducts();
		void MarkAllProductsAsCached();

		HashSet<int> DeletedProducts { get; }
		bool IsProductDeleted(int id);
		void MarkProductAsDeleted(int id);
	}
}
