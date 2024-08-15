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

			builder.HasKey(user => user.Id);
			builder.OwnsOne(user => user.Address, address =>
			{
				address.Property(p => p.Street);
				address.Property(p => p.City);
				address.Property(p => p.State);
				address.Property(p => p.Country);
				address.Property(p => p.ZipCode);
			});
			builder.Property(user => user.BusinessName).HasMaxLength(100);
			builder.Property(user => user.CorporateName).HasMaxLength(100);
		}
	}
}
