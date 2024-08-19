using Store.Domain.Entity;

namespace Store.Domain.Interface.Infra.Adapters
{
    public interface IProductService
    {
        Task<Product?> FetchProduct(int id);
        Task<List<Product>> FetchProductsFromCategory(string category);
    }
}
