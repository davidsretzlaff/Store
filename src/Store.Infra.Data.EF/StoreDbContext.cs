using Microsoft.EntityFrameworkCore;
using Store.Domain.Entity;
using Store.Infra.Data.EF.Configurations;
using Store.Infra.Data.EF.Models;

namespace Store.Infra.Data.EF
{
	public class StoreDbContext : DbContext
	{
		public DbSet<User> Users => Set<User>();
		public DbSet<Order> Orders => Set<Order>();
		public DbSet<Delivery> Deliveries => Set<Delivery>();
		public DbSet<OrdersProducts> OrdersProducts => Set<OrdersProducts>();

		public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new UserConfiguration());
			builder.ApplyConfiguration(new OrderConfiguration());
			builder.ApplyConfiguration(new OrdersProductsConfiguration());
			builder.ApplyConfiguration(new DeliveryConfiguration());
		}
	}
}
