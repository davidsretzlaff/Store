using Store.Domain.Entity;

namespace Store.Domain.Repository
{
	public interface IProductRepository : IGenericRepository<Product>, ISearchableRepository<Product>
	{
		public Task<Product?> Get(int id, CancellationToken cancellationToken);
	}
}
