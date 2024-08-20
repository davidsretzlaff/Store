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
		public string CompanyIdentificationNumber { get; set; }

		public SearchInput(
			int page,
			int perPage,
			string search,
			string orderBy,
			SearchOrder order,
			string companyIdentificationNumber
		)
		{
			Page = page;
			PerPage = perPage;
			Search = search;
			OrderBy = orderBy;
			Order = order;
			CompanyIdentificationNumber = companyIdentificationNumber;
		}
	}
}
