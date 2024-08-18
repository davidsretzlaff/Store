using Store.Domain.Extensions;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Product.Common
{
	public record CreateProductOutput
	(
		int Id,
		string Title,
		string Price,
		string Description,
		string Category
	)
	{
		public static CreateProductOutput FromProduct( DomainEntity.Product product)
		{
			return new CreateProductOutput(
				product.Id, 
				product.Title,
				product.GetPriceAsCurrency(),
				product.Description,
				product.Category.ToCategoryString()
				);
		}
	}
}
