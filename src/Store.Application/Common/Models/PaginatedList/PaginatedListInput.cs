using Store.Domain.Enum;
using Store.Domain.SeedWork.Searchable;
using System.Security.Cryptography.X509Certificates;

namespace Store.Application.Common.Models.PaginatedList
{
	public abstract class PaginatedListInput
	{
		public int Page { get; set; }
		public int PerPage { get; set; }
		public string Search { get; set; }
		public string OrderBy { get; set; }
		public SearchOrder Order { get; set; }
		public string? User {  get; set; }
		public PaginatedListInput(
			int page,
			int perPage,
			string search,
			string orderBy,
			SearchOrder order,
			string? user
		)
		{
			Page = page;
			PerPage = perPage;
			Search = search;
			OrderBy = orderBy;
			Order  = order;
			User = user;
		}

		public SearchInput ToSearchInput()
			=> new(Page, PerPage, Search, OrderBy, Order, User);
	}
}
