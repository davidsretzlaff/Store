using Store.Domain.SeedWork;
namespace Store.Domain.Interface.Repository
{
    public interface IGenericRepository<TAggregate> : IRepository
        where TAggregate : AggregateRoot
    {
        public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
        public Task Delete(TAggregate aggregate, CancellationToken cancellationToken);
        public Task Update(TAggregate aggregate, CancellationToken cancellationToken);
    }
}
