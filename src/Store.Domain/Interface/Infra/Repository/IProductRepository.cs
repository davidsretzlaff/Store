using Store.Domain.Entity;

namespace Store.Domain.Interface.Infra.Repository
{
    public interface IProductRepository : IGenericRepository<Product>, ISearchableRepository<Product>
    {
        public Task<Product?> Get(int id, bool includeDeleted, CancellationToken cancellationToken);
        public Task Delete(Product aggregate, CancellationToken cancellationToken);

        public Task<IReadOnlyList<Product>> GetListByIds(List<int> ids, bool includeDeleted, CancellationToken cancellationToken);
    }
}
