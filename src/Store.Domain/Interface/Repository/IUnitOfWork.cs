namespace Store.Domain.Interface.Repository
{
    public interface IUnitOfWork
    {
        public Task Commit(CancellationToken cancellationToken);
        public Task Rollback(CancellationToken cancellationToken);
    }
}

