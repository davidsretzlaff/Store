
using Store.Application.Common.ApiResponse.PaginatedList;

namespace Store.Application.Common.Models.Response
{
	public class ResponseList<TItemData> : Response<IReadOnlyList<TItemData>>
	{
		public ResponseListMeta Meta { get; private set; }

		public ResponseList(
			int currentPage,
			int perPage,
			int total,
			IReadOnlyList<TItemData> data
		) : base(data)
		{
			Meta = new ResponseListMeta(currentPage, perPage, total);
		}

		public ResponseList(PaginatedListOutput<TItemData> paginatedListOutput) : base(paginatedListOutput.Items)
		{
			Meta = new ResponseListMeta(
				paginatedListOutput.Page,
				paginatedListOutput.PerPage,
				paginatedListOutput.Total
			);
		}
	}

}
