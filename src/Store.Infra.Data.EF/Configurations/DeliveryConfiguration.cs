using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entity;

namespace Store.Infra.Data.EF.Configurations
{
	public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
	{
		public void Configure(EntityTypeBuilder<Delivery> builder)
		{
			builder.HasKey(e => e.OrderId);
			builder.Property(u => u.CompanyIdentificationNumber).IsRequired().HasMaxLength(50);
			builder.Property(u => u.Status).HasConversion<string>().IsRequired();
			builder.Ignore(d => d.Order);
			builder.OwnsOne(u => u.Address, a =>
			{
				a.Property(ad => ad.Street)
					.IsRequired()
					.HasMaxLength(150);

				a.Property(ad => ad.City)
					.IsRequired()
					.HasMaxLength(100);

				a.Property(ad => ad.State)
					.IsRequired()
					.HasMaxLength(50);

				a.Property(ad => ad.ZipCode)
					.IsRequired()
					.HasMaxLength(10);
			});
		}
	}
}
