using Store.Domain.Enum;

namespace Store.Domain.SeedWork.Searchable
{
	public class SearchInput
	{
		public int Page { get; set; }
		public int PerPage { get; set; }
		public string Search { get; set; }
		public string OrderBy { get; set; }
		public SearchOrder Order { get; set; }
		public string Cnpj { get; set; }

		public SearchInput(
			int page,
			int perPage,
			string search,
			string orderBy,
			SearchOrder order,
			string Cnpj
		)
		{
			Page = page;
			PerPage = perPage;
			Search = search;
			OrderBy = orderBy;
			Order = order;
			this.Cnpj = Cnpj;
		}
	}
}
