using MediatR;
using Store.Application.Common.Models.PaginatedList;
using Store.Application.UseCases.Order.ListOrders;
using Store.Domain.Enum;

namespace Store.Application.UseCases.Delivery.ListDeliveries
{
	public class ListDeliveriesInput : PaginatedListInput, IRequest<ListDeliveriesOutput>
	{

		public ListDeliveriesInput(int page, int perPage, string search, string orderBy, SearchOrder dir, string Cnpj)
			: base(page, perPage, search, orderBy, dir, Cnpj)
		{
		}

		public ListDeliveriesInput(string Cnpj) : base(1, 10, "", "", SearchOrder.Asc, Cnpj)
		{
		}
	}
}
