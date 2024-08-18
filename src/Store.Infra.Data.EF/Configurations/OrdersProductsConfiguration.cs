using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Infra.Data.EF.Models;

namespace Store.Infra.Data.EF.Configurations
{
	public class OrdersProductsConfiguration : IEntityTypeConfiguration<OrdersProducts>
	{
		public void Configure(EntityTypeBuilder<OrdersProducts> builder)
		{
			builder.HasKey(relation => new {
				relation.OrderId,
				relation.ProductId
			});
		}
	}
}
