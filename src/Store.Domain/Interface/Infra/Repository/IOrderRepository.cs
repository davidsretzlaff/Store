using Store.Domain.Entity;

namespace Store.Domain.Interface.Infra.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>, ISearchableRepository<Order>
    {
        public Task<Order?> Get(string id, CancellationToken cancellationToken);

        public Task Update(Order order, CancellationToken cancellationToken);
    }
}
