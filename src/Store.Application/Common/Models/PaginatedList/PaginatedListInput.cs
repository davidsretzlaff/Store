using Store.Domain.Enum;
using Store.Domain.SeedWork.Searchable;

namespace Store.Application.Common.Models.PaginatedList
{
	public abstract class PaginatedListInput
	{
		public int Page { get; set; }
		public int PerPage { get; set; }
		public string Search { get; set; }
		public string OrderBy { get; set; }
		public SearchOrder Order { get; set; }
		public PaginatedListInput(
			int page,
			int perPage,
			string search,
			string orderBy,
			SearchOrder order)
		{
			Page = page;
			PerPage = perPage;
			Search = search;
			OrderBy = orderBy;
			Order  = order;
		}

		public SearchInput ToSearchInput()
			=> new(Page, PerPage, Search, OrderBy, Order);
	}
}
