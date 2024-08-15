using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entity;

namespace Store.Infra.Data.EF.Configurations
{
	internal class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(user => user.Id);
			builder.Property(user => user.BusinessName).HasMaxLength(100);
			builder.Property(user => user.CorporateName).HasMaxLength(100);
		}
	}
}
