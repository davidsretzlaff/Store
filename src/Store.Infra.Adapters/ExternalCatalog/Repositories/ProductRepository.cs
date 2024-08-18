using Store.Application.Common.Interface;
using Store.Domain.Entity;
using Store.Domain.Repository;
using Store.Domain.SeedWork.Searchable;
using Store.Infra.Adapters.ExternalCatalog.Models;

namespace Store.Infra.Adapters.ExternalCatalog.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IApiClient _apiClient;

        public ProductRepository(IApiClient apiClient) => _apiClient = apiClient;

        public Task Delete(Product aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> Get(int id, CancellationToken cancellationToken)
        {
            var (response, product) = await _apiClient.Get<ProductDto>($"/products/{id}");
            return product.ToProduct();
        }

        public Task Insert(Product aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<SearchOutput<Product>> Search(SearchInput input, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Update(Product aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
