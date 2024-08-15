using Store.Domain.Entity;
using Store.Domain.SeedWork.Searchable;

namespace Store.Domain.Repository
{
	public interface IUserRepository : IGenericRepository<User>, ISearchableRepository<User>
	{
	}
}
