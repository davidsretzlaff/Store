using MediatR;
using Store.Application.Common.Models.PaginatedList;
using Store.Domain.Enum;

namespace Store.Application.UseCases.Product.ListProducts
{
	public class ListProductsInput : PaginatedListInput, IRequest<ListProductsOutput>
	{
		public ListProductsInput(int page, int perPage, string search, string orderBy, SearchOrder dir, string? user)
		: base(page, perPage, search, orderBy, dir, user)
		{
		}

		public ListProductsInput(string? user) : base(1, 10, "", "", SearchOrder.Asc, user)
		{
		}
	}
}
