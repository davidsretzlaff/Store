using Store.Domain.Entity;
namespace Store.Domain.Repository
{
	public interface IUserRepository : IGenericRepository<User>, ISearchableRepository<User>
	{
		public Task<User> GetByUserName(string userName, CancellationToken cancellationToken);
	}
}
