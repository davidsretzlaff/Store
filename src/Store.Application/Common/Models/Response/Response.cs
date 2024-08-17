namespace Store.Application.Common.Models.Response
{
    public class Response<TData>
    {
        public Response(TData data) => Data = data;

        public TData Data { get; private set; }
    }
}
