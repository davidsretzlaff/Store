using Store.Domain.Entity;
namespace Store.Domain.Repository
{
	public interface IUserRepository : IGenericRepository<User>, ISearchableRepository<User>
	{
		public Task<User?> GetByUserNameOrCompanyRegNumber(string userName, string companyRegNumber, CancellationToken cancellationToken);
		public Task<User?> GetByUserName(string userName, CancellationToken cancellationToken);
		public Task<User> Get(Guid id, CancellationToken cancellationToken);
	}
}
