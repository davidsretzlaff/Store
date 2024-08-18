using Store.Application.Common.ApiResponse.PaginatedList;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Product.ListProducts
{
	public class ListProductsOutput : PaginatedListOutput<ProductOutput>
	{
		public ListProductsOutput(int page, int perPage, int total, IReadOnlyList<ProductOutput> items)
			: base(page, perPage, total, items)
		{
		}
	}
}
