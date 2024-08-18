using Store.Application.Common.Exceptions;
using Store.Application.Common.Interface;
using Store.Application.Common.Models.Response;
using Store.Application.UseCases.User.Common;
using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.Repository;
using Store.Domain.SeedWork.Searchable;
using Store.Infra.Adapters.ExternalCatalog.Models;
using System.Net;

namespace Store.Infra.Adapters.ExternalCatalog.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IApiClient _apiClient;
		private readonly HashSet<int> _deletedProducts;
		private readonly Dictionary<int, CachedProduct> _productCache;
		private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(3);

		public ProductRepository(IApiClient apiClient) 
        {
			_apiClient = apiClient;
			_deletedProducts = new HashSet<int>();
			_productCache = new Dictionary<int, CachedProduct>();

		} 

        public Task Delete(Product product, CancellationToken cancellationToken)
        {
			_deletedProducts.Add(product.Id);
			_productCache.Remove(product.Id);
			return Task.CompletedTask;
		}

        public async Task<Product> Get(int id, CancellationToken cancellationToken)
        {
			if (_deletedProducts.Contains(id))
			{
				throw new NotFoundException($"Product '{id}' has been deleted.");
			}
			if (_productCache.TryGetValue(id, out var cachedProduct) && 
				!cachedProduct.IsExpired(_cacheExpiration))
			{
				return cachedProduct.Product;
			}

			var (response, productDto) = await _apiClient.Get<ApiProduct>($"/products/{id}");
			var product = productDto.ToProduct();
			_productCache[id] = new CachedProduct(product, DateTime.UtcNow);
			return product;
        }

        public async Task Insert(Product input, CancellationToken cancellationToken)
        {
			var (response, product) = await _apiClient.Post<Response<ApiProduct>>($"/products/", input);
			if (response is not null && 
				response.StatusCode == HttpStatusCode.Created)
			{
				_productCache[input.Id] = new CachedProduct(input, DateTime.UtcNow);
			}
		}

        public async Task<SearchOutput<Product>> Search(SearchInput input, CancellationToken cancellationToken)
        {
			var (response, productDto) = await _apiClient.Get<List<ApiProduct>>($"/products/");

			foreach (var item in productDto)
			{
				CacheProduct(item);
			}

			var toSkip = (input.Page - 1) * input.PerPage;
			var query = _productCache.Values.Select(cp => cp.Product).AsQueryable();
			query = AddOrderToQuery(query, input.OrderBy, input.Order);

			if (!string.IsNullOrWhiteSpace(input.Search))
			{
				var searchToLower = input.Search.ToLower();
				query = query.Where(x =>
					x.Title.ToLower().Contains(searchToLower) ||
					x.Description.ToLower().StartsWith(searchToLower) ||
					x.Category.ToCategoryString().ToLower().Contains(searchToLower)
				);
			}

			var total = query.Count();
			var items = query
				.Skip(toSkip)
				.Take(input.PerPage)
				.ToList();

			return new SearchOutput<Product>(input.Page, input.PerPage, total, items);
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

		private void CacheProduct(ApiProduct item)
		{
			if (_deletedProducts.Contains(item.Id))
			{
				return;
			}

			if (AllowedCategory(item))
			{
				_productCache[item.Id] = new CachedProduct(item.ToProduct(), DateTime.UtcNow);
			}
		}

		private bool AllowedCategory(ApiProduct item)
		{
			var allowedCategories = new[] { Category.Electronics, Category.Jewelery };
			return allowedCategories.Contains(item.Category.ToCategory());
		}
	}
}
