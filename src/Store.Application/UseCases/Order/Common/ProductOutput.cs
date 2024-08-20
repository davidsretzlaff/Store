using Store.Domain.Extensions;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.Common
{
	public record ProductOutput
	(
		 int Id,
		 string Title,
		 string Description,
		 string Price,
		 string Category
	)
	{
		public static ProductOutput FromProduct(DomainEntity.Product product)
		{
			return new ProductOutput(
				product.ProductId,
				product.Title,
				product.Description,
				product.GetPriceAsCurrency(),
				product.Category.ToCategoryString()
				);
		}
	}
}
