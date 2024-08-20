using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entity;

namespace Store.Infra.Data.EF.Configurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(u => u.Status).HasConversion<string>().IsRequired();
			builder.OwnsOne(u => u.Cnpj, a =>
			{
				a.Property(ad => ad.Value).IsRequired();
			});
			builder.OwnsOne(u => u.CustomerDocument, a =>
			{
				a.Property(ad => ad.Value).IsRequired();
			});
		}
	}
}
