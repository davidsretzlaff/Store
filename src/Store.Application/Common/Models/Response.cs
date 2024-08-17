namespace Store.Application.Common.ApiModels.NewFolder
{
	public class ApiResponse<TData>
	{
		public ApiResponse(TData data) => Data = data;

		public TData Data { get; private set; }
	}
}
