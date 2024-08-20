using Store.Domain.Extensions;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Product.CreateProduct
{
	public record CreateProductOutput
	(
		 int Id,
		 string Title,
		 string Description,
		 string Price,
		 string Category
	)
	{
		public static CreateProductOutput FromProduct(DomainEntity.Product product)
		{
			return new CreateProductOutput(
				product.ProductId,
				product.Title,
				product.Description,
				product.GetPriceAsCurrency(),
				product.Category.ToCategoryString()
			);
		}
	}
}
