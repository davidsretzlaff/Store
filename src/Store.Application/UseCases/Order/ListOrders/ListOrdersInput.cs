using MediatR;
using Store.Application.Common.Models.PaginatedList;
using Store.Domain.Enum;

namespace Store.Application.UseCases.Order.ListOrders
{
	public class ListOrdersInput : PaginatedListInput, IRequest<ListOrdersOutput>
	{
		public ListOrdersInput(int page, int perPage, string search, string orderBy, SearchOrder dir, string? user)
			: base(page, perPage, search, orderBy, dir, user)
		{
		}

		public ListOrdersInput(string? user) : base(1, 10, "", "", SearchOrder.Asc, user)
		{
		}
	}
}
