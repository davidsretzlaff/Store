using Store.Application.Common.Models.Response;
using Store.Domain.Entity;
using Store.Domain.Interface.Infra.Adapters;
using Store.Infra.Adapters.ExternalCatalog.Models;

namespace Store.Infra.Adapters.ExternalCatalog
{
    public class ProductService : IProductService
	{
		private readonly IApiClient _apiClient;

		public ProductService(IApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		public async Task<Product?> FetchProduct(int id)
		{
			var (response, productDto) = await _apiClient.Get<ProductDto>($"/products/{id}");
			if (IsApiProductDtoValid(response, productDto))
			{
				return productDto.ToProduct();
			}
			return null;
		}

		public async Task<List<Product>> FetchProductsFromCategory(string category)
		{
			var (response, responseList) = await _apiClient.Get<List<ProductDto>>($"/products/{category}");
			if (IsApiProductListResponseValid(response, responseList))
			{
				return responseList!.Select(dto => dto.ToProduct()).ToList();
			}
			return new List<Product>();
		}

		private bool IsApiProductDtoValid(HttpResponseMessage? response, ProductDto? product)
		{
			return IsValidResponse(response) && product != null;
		}

		private bool IsApiProductListResponseValid(HttpResponseMessage? response, List<ProductDto>? products)
		{
			return
				IsValidResponse(response) &&
				products is not null &&
				products.Any();
		}

		private bool IsValidResponse(HttpResponseMessage? response)
		{
			return response is not null && response.IsSuccessStatusCode;
		}
	}
}
