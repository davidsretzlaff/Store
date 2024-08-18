using Microsoft.EntityFrameworkCore;
using Store.Application.Common.Exceptions;
using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Interface.Repository;
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

		public async Task Update(User user, CancellationToken cancellationToken)
		{
			await Task.FromResult(_users.Update(user));
		}

		public async Task<User> Get(Guid id, CancellationToken cancellationToken)
		{
			var user = await _users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
			NotFoundException.ThrowIfNull(user, $"User '{id}' not found.");
			return user!;
		}

		public Task Delete(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(_users.Remove(user));
		}

		public async Task<SearchOutput<User>> Search(SearchInput input, CancellationToken cancellationToken)
		{
			var toSkip = (input.Page - 1) * input.PerPage;
			var query = _users.AsNoTracking();
			query = AddOrderToQuery(query, input.OrderBy, input.Order);

			if (!string.IsNullOrWhiteSpace(input.Search))
			{
				var searchToLower = input.Search.ToLower();
				query = query.Where(x =>
					x.UserName.ToLower().Contains(searchToLower) ||
					x.BusinessName.ToLower().StartsWith(searchToLower) ||
					x.CorporateName.ToLower().Contains(searchToLower) ||
					x.CompanyRegistrationNumber.ToLower().Contains(searchToLower)
				);
			}

			var total = await query.CountAsync();
			var items = await query
				.Skip(toSkip)
				.Take(input.PerPage)
				.ToListAsync();

			return new SearchOutput<User>(input.Page, input.PerPage, total, items);
		}

		private IQueryable<User> AddOrderToQuery(IQueryable<User> query, string orderProperty,SearchOrder order)
		{
			var orderedQuery = (orderProperty.ToLower(), order) switch
			{
				("username", SearchOrder.Asc) => query.OrderBy(x => x.UserName).ThenBy(x => x.Id),
				("username", SearchOrder.Desc) => query.OrderByDescending(x => x.UserName).ThenByDescending(x => x.Id),
				("businessname", SearchOrder.Asc) => query.OrderBy(x => x.BusinessName).ThenBy(x => x.Id),
				("businessname", SearchOrder.Desc) => query.OrderByDescending(x => x.BusinessName).ThenByDescending(x => x.Id),
				("corporatename", SearchOrder.Asc) => query.OrderBy(x => x.CorporateName).ThenBy(x => x.Id),
				("corporatename", SearchOrder.Desc) => query.OrderByDescending(x => x.CorporateName).ThenByDescending(x => x.Id),
				("email", SearchOrder.Asc) => query.OrderBy(x => x.Email).ThenBy(x => x.Id),
				("email", SearchOrder.Desc) => query.OrderByDescending(x => x.Email).ThenByDescending(x => x.Id),
				_ => query.OrderBy(x => x.BusinessName).ThenBy(x => x.Id)
			};
			return orderedQuery;
		}

		//david criar test
		public async Task<User?> GetByUserNameOrCompanyRegNumber(string userName, string? companyRegNumber, CancellationToken cancellationToken)
		{
			var user = await _users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.UserName == userName || x.CompanyRegistrationNumber == companyRegNumber, cancellationToken);
			return user;
		}

		public async Task<User?> GetByUserName(string userName, CancellationToken cancellationToken)
		{
			var user = await _users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);
			return user;
		}
	}
}
