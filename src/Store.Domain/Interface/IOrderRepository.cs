using Store.Domain.Entity;

namespace Store.Domain.Repository
{
	public interface IOrderRepository : IGenericRepository<Order>, ISearchableRepository<Order>
	{
		public Task<Order> Get(Guid id, CancellationToken cancellationToken);
	}
}
