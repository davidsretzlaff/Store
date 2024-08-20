using MediatR;
using Store.Application.Common.Models.PaginatedList;
using Store.Application.UseCases.Order.ListOrders;
using Store.Domain.Enum;

namespace Store.Application.UseCases.Delivery.ListDeliveries
{
	public class ListDeliveriesInput : PaginatedListInput, IRequest<ListDeliveriesOutput>
	{
		public ListDeliveriesInput(int page, int perPage, string search, string orderBy, SearchOrder dir, string? user)
			: base(page, perPage, search, orderBy, dir, user)
		{
		}

		public ListDeliveriesInput(string? user) : base(1, 10, "", "", SearchOrder.Asc, user)
		{
		}
	}
}
