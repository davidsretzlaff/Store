using Store.Application.Common.ApiResponse.PaginatedList;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Order.ListOrders
{
	public class ListOrdersOutput : PaginatedListOutput<OrderOutput>
	{
		public ListOrdersOutput(int page, int perPage, int total, IReadOnlyList<OrderOutput> items)
			: base(page, perPage, total, items)
		{
		}
	}
}
