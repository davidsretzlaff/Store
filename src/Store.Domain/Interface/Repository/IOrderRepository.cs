using Store.Domain.Entity;
using Store.Domain.Repository;

namespace Store.Domain.Interface.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>, ISearchableRepository<Order>
    {
		public Task<Order?> Get(string id, CancellationToken cancellationToken);

		public Task Update(Order order, CancellationToken cancellationToken);
	}
}
