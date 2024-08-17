namespace Store.Application.Common.Models.Response
{
	public class ResponseListMeta
	{
		public int CurrentPage { get; set; }
		public int PerPage { get; set; }
		public int Total { get; set; }

		public ResponseListMeta(int currentPage, int perPage, int total)
		{
			CurrentPage = currentPage;
			PerPage = perPage;
			Total = total;
		}
	}
}
