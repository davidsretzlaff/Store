using Store.Domain.Entity;
using Store.Domain.Extensions;

namespace Store.Application.UseCases.Order.Common
{
	public record ItemOutput
	(
		 int Id,
		 string Title,
		 string Description,
		 string Price,
		 string Category,
		 int quantity
	)
	{
		public static ItemOutput FromProduct(Item item)
		{
			return new ItemOutput(
				item.Product.Id,
				item.Product.Title,
				item.Product.Description,
				item.Product.GetPriceAsCurrency(),
				item.Product.Category.ToCategoryString(),
				item.Quantity
				);
		}
	}
}
