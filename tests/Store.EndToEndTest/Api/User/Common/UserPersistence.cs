using Microsoft.EntityFrameworkCore;
using Store.Infra.Data.EF;

namespace Store.EndToEndTest.Api.User.Common
{
	public class UserPersistence
	{
		private readonly StoreDbContext _context;

		public UserPersistence(StoreDbContext context) => _context = context;

		public async Task<Domain.Entity.User?> GetById(Guid id)
			=> await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

		public async Task InsertList(List<Domain.Entity.User> categories)
		{
			await _context.Users.AddRangeAsync(categories);
			await _context.SaveChangesAsync();
		}
	}
}
