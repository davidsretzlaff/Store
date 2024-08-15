using Microsoft.EntityFrameworkCore;
using Store.Domain.Entity;
using Store.Infra.Data.EF.Configurations;

namespace Store.Infra.Data.EF
{
	public class StoreDbContext : DbContext
	{
		public DbSet<User> Users => Set<User>();
		public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new UserConfiguration());
		}
	}
}
