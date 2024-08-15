using Microsoft.EntityFrameworkCore;
using Store.Domain.Entity;
using Store.Domain.Repository;
using Store.Domain.SeedWork.Searchable;

namespace Store.Infra.Data.EF.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly StoreDbContext _context;
		private DbSet<User> _users => _context.Set<User>();
		public UserRepository(StoreDbContext context) => _context = context;

		public async Task Insert(User user, CancellationToken cancellationToken)
		{
			await _users.AddAsync(user, cancellationToken);
		}

		public Task Delete(User aggregate, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<User> Get(Guid id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}


		public Task<SearchOutput<User>> Search(SearchInput input, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task Update(User aggregate, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
