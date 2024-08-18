using Store.Domain.Entity;

namespace Store.Domain.Interface
{
	public interface IProductService
	{
		Task<Product?> FetchProduct(int id);
		Task<List<Product>> FetchProductsFromCategory(string category);
	}
}
