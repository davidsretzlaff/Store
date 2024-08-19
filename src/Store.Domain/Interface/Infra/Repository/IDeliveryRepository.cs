
using Store.Domain.Entity;

namespace Store.Domain.Interface.Infra.Repository
{
	public interface IDeliveryRepository : IGenericRepository<Delivery>, ISearchableRepository<Delivery>
	{
		public Task<Delivery?> Get(string orderId, CancellationToken cancellationToken);

		public Task Update(Delivery order, CancellationToken cancellationToken);
	}
}
