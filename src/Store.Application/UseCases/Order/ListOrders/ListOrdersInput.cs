using MediatR;
using Store.Application.Common.Models.PaginatedList;
using Store.Application.UseCases.Product.ListProducts;
using Store.Domain.Enum;

namespace Store.Application.UseCases.Order.ListOrders
{
	public class ListOrdersInput : PaginatedListInput, IRequest<ListOrdersOutput>
	{
		public ListOrdersInput(int page, int perPage, string search, string orderBy, SearchOrder dir)
		: base(page, perPage, search, orderBy, dir)
		{
		}

		public ListOrdersInput() : base(1, 10, "", "", SearchOrder.Asc)
		{ }
	}
}
