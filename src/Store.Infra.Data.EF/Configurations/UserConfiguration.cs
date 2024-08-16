using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entity;
using Store.Domain.ValueObject;
using System.Reflection.Emit;

namespace Store.Infra.Data.EF.Configurations
{
	internal class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.Property(u => u.UserName).IsRequired().HasMaxLength(100);
			builder.Property(u => u.BusinessName).IsRequired().HasMaxLength(100);
			builder.Property(u => u.CorporateName).IsRequired().HasMaxLength(100);
			builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
			builder.Property(u => u.SiteUrl).HasMaxLength(100);
			builder.Property(u => u.Phone).HasMaxLength(20);
			builder.Property(u => u.CompanyRegistrationNumber).IsRequired().HasMaxLength(50);
			builder.Property(u => u.Status).HasConversion<string>().IsRequired();

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
