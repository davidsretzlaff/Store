using Store.Application.Common.ApiResponse.PaginatedList;
using Store.Application.UseCases.Delivery.Common;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Delivery.ListDeliveries
{
	public class ListDeliveriesOutput : PaginatedListOutput<DeliveryOutput>
	{
		public ListDeliveriesOutput(int page, int perPage, int total, IReadOnlyList<DeliveryOutput> items)
			: base(page, perPage, total, items)
		{
		}
	}
}
