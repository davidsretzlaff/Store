using Store.Application.Common.Interface;

namespace Store.Infra.Data.EF
{
    public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreDbContext _context;

		public UnitOfWork(StoreDbContext context)
		{
			_context = context;
		}

		public Task Commit(CancellationToken cancellationToken)
			=> _context.SaveChangesAsync(cancellationToken);

		public Task Rollback(CancellationToken cancellationToken)
			=> Task.CompletedTask;
	}
}
