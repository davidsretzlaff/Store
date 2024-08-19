namespace Store.Domain.Interface.Infra.Adapters
{
    public interface IApiClient
    {
        Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
            string route,
            object payload
        ) where TOutput : class;

        Task<(HttpResponseMessage?, TOutput?)> Put<TOutput>(
            string route,
            object? payload = null
        ) where TOutput : class;

        Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(
            string route,
            object? queryStringParametersObject = null
        ) where TOutput : class;

        Task<(HttpResponseMessage?, TOutput?)> Delete<TOutput>(
            string route
        ) where TOutput : class;
    }
}
