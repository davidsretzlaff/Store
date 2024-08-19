using Store.Domain.Entity;
using Store.Domain.Repository;

namespace Store.Domain.Interface.Repository
{
    public interface IProductRepository : IGenericRepository<Product>, ISearchableRepository<Product>
    {
        public Task<Product?> Get(int id, CancellationToken cancellationToken);
		public Task Delete(Product aggregate, CancellationToken cancellationToken);

		public Task<IReadOnlyList<Product>> GetListByIds(List<int> ids, CancellationToken cancellationToken);
	}
}
