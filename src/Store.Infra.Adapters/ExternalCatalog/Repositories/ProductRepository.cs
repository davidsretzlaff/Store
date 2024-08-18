using Store.Application.Common.Interface;
using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.Repository;
using Store.Domain.SeedWork.Searchable;
using Store.Infra.Adapters.ExternalCatalog.Models;

namespace Store.Infra.Adapters.ExternalCatalog.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IApiClient _apiClient;
		private readonly HashSet<int> _deletedProducts;
		private readonly Dictionary<int, CachedProduct> _productCache;
		private bool _allProductsCached = false;
		private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(3);

		public ProductRepository(IApiClient apiClient, ICacheService cacheService) 
        {
			_apiClient = apiClient;
			_deletedProducts = new HashSet<int>();
			_productCache = new Dictionary<int, CachedProduct>();
		}

		public Task Insert(Product input, CancellationToken cancellationToken)
		{
			CacheProduct(input);
			return Task.CompletedTask;
		}

		public Task Delete(Product product, CancellationToken cancellationToken)
        {
			_deletedProducts.Add(product.Id);
			_productCache.Remove(product.Id);
			return Task.CompletedTask;
		}

		public async Task<Product?> Get(int id, CancellationToken cancellationToken)
		{
			if (IsProductDeleted(id))
			{
				return null;
			}
			if (TryGetCachedProduct(id, out var cachedProduct))
			{
				return cachedProduct.Product;
			}
			return await FetchAndCacheProduct(id);
		}

		public async Task<SearchOutput<Product>> Search(SearchInput input, CancellationToken cancellationToken)
		{
			await EnsureProductsCachedAsync();

			var query = BuildQueryFromCache(input);
			query = ApplySearchFilter(query, input.Search);

			var total = query.Count();
			var items = query
				.Skip(GetSkipAmount(input))
				.Take(input.PerPage)
				.ToList();

			return new SearchOutput<Product>(input.Page, input.PerPage, total, items);
		}

		private async Task EnsureProductsCachedAsync()
		{
			if (!_allProductsCached)
			{
				await CacheProductsFromCategoryAsync("category/electronics");
				await CacheProductsFromCategoryAsync("category/jewelery");
				_allProductsCached = true;
			}
		}

		private IQueryable<Product> BuildQueryFromCache(SearchInput input)
		{
			var query = _productCache.Values.Select(cp => cp.Product).AsQueryable();
			return AddOrderToQuery(query, input.OrderBy, input.Order);
		}

		private IQueryable<Product> ApplySearchFilter(IQueryable<Product> query, string search)
		{
			if (string.IsNullOrWhiteSpace(search))
			{
				return query;
			}

			var searchToLower = search.ToLower();
			return query.Where(x =>
				x.Title.ToLower().Contains(searchToLower) ||
				x.Description.ToLower().StartsWith(searchToLower) ||
				x.Category.ToCategoryString().ToLower().Contains(searchToLower)
			);
		}

		private int GetSkipAmount(SearchInput input)
		{
			return (input.Page - 1) * input.PerPage;
		}
		
		private async Task CacheProductsFromCategoryAsync(string category)
		{
			var (response, productDto) = await _apiClient.Get<List<ApiProduct>>($"/products/{category}");

			if (response.IsSuccessStatusCode)
			{
				foreach (var item in productDto)
				{
					CacheProduct(item.ToProduct());
				}
			}
		}

		public Task Update(Product aggregate, CancellationToken cancellationToken)
        {
			//_productCache[aggregate.Id] = new CachedProduct(aggregate, DateTime.UtcNow);
			//return Task.CompletedTask;
			throw new NotImplementedException();
        }

		//david melhorar
		public class CachedProduct
		{
			public Product Product { get; }
			public DateTime Timestamp { get; }
			public CachedProduct(Product product, DateTime timestamp)
			{
				Product = product ?? throw new ArgumentNullException(nameof(product));
				Timestamp = timestamp;
			}

			public bool IsExpired(TimeSpan cacheExpiration) =>
				(DateTime.UtcNow - Timestamp) >= cacheExpiration;
		}

		private IQueryable<Product> AddOrderToQuery(IQueryable<Product> query, string orderProperty, SearchOrder order)
		{
			var orderedQuery = (orderProperty.ToLower(), order) switch
			{
				("title", SearchOrder.Asc) => query.OrderBy(x => x.Title).ThenBy(x => x.Id),
				("title", SearchOrder.Desc) => query.OrderByDescending(x => x.Title).ThenByDescending(x => x.Id),
				("description", SearchOrder.Asc) => query.OrderBy(x => x.Description).ThenBy(x => x.Id),
				("description", SearchOrder.Desc) => query.OrderByDescending(x => x.Description).ThenByDescending(x => x.Id),
				_ => query.OrderBy(x => x.Title).ThenBy(x => x.Id)
			};
			return orderedQuery;
		}

		private bool AllowedCategory(ApiProduct item)
		{
			var allowedCategories = new[] { Category.Electronics, Category.Jewelery };
			return allowedCategories.Contains(item.Category.ToCategory());
		}

		private bool IsProductInCache(int productId)
		{
			return _productCache.ContainsKey(productId);
		}

		private void CacheProduct(Product product)
		{
			if (_deletedProducts.Contains(product.Id))
			{
				return;
			}
			if (!IsProductInCache(product.Id))
			{
				_productCache[product.Id] = new CachedProduct(product, DateTime.UtcNow);
			}
		}

		private bool IsProductDeleted(int id)
		{
			return _deletedProducts.Contains(id);
		}

		private bool TryGetCachedProduct(int id, out CachedProduct cachedProduct)
		{
			return _productCache.TryGetValue(id, out cachedProduct) &&
				   !cachedProduct.IsExpired(_cacheExpiration);
		}

		private async Task<Product?> FetchAndCacheProduct(int id)
		{
			var (response, productDto) = await _apiClient.Get<ApiProduct>($"/products/{id}");

			if (IsApiResponseValid(response, productDto))
			{
				var product = productDto.ToProduct();
				CacheProduct(product);
				return product;
			}
			return null;
		}

		private bool IsApiResponseValid(HttpResponseMessage? response, ApiProduct? dto)
		{
			return response is not null &&
				   response.IsSuccessStatusCode &&
				   dto is not null;
		}

	}
}
