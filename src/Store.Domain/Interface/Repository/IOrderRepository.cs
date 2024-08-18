using Store.Domain.Entity;
using Store.Domain.Repository;

namespace Store.Domain.Interface.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>, ISearchableRepository<Order>
    {
        public Task<Order> Get(Guid id, CancellationToken cancellationToken);

		public Task Update(Order aggregate, CancellationToken cancellationToken);
	}
}
