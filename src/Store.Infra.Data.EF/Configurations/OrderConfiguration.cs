using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entity;

namespace Store.Infra.Data.EF.Configurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(u => u.CompanyRegisterNumber).IsRequired().HasMaxLength(50);
			builder.Property(u => u.Status).HasConversion<string>().IsRequired();
		}
	}
}
