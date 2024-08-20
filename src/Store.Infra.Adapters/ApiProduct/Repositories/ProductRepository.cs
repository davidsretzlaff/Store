using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.Interface.Infra.Adapters;
using Store.Domain.Interface.Infra.Repository;
using Store.Domain.SeedWork.Searchable;

namespace Store.Infra.Adapters.ExternalCatalog.Repositories
{
    public class ProductRepository : IProductRepository
	{
		private readonly IProductService _productService;
		private readonly ICacheService _cacheService;


		public ProductRepository(IProductService productService, ICacheService cacheService)
		{
			_productService = productService;
			_cacheService = cacheService;
		}

		public Task Insert(Product input, CancellationToken cancellationToken)
		{
			_cacheService.CacheProduct(input);
			return Task.CompletedTask;
		}

		public Task Delete(Product product, CancellationToken cancellationToken)
		{
			_cacheService.MarkProductAsDeleted(product.ProductId);
			_cacheService.RemoveProductFromCache(product.ProductId);
			return Task.CompletedTask;
		}

		public async Task<Product?> Get(int id, bool includeDeleted, CancellationToken cancellationToken)
		{
			if (ShouldExcludeDeletedProduct(id, includeDeleted))
			{
				return null;
			}

			var cachedProduct = _cacheService.GetCachedProduct(id);
			if (cachedProduct != null)
			{
				return cachedProduct;
			}
			return await FetchAndCacheProduct(id);
		}

		public async Task<IReadOnlyList<Product>> GetListByIds(List<int> ids, bool includeDeleted, CancellationToken cancellationToken)
		{
			var result = new List<Product>();
			foreach (var id in ids)
			{
				var product = await Get(id, includeDeleted, cancellationToken);
				if (product != null)
				{
					result.Add(product);
				}
			}
			return result;
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
			if (!_cacheService.AllProductsCached)
			{
				await CacheProductsFromCategoryAsync("category/electronics");
				await CacheProductsFromCategoryAsync("category/jewelery");
				_cacheService.MarkAllProductsAsCached();
			}
		}

		private IQueryable<Product> BuildQueryFromCache(SearchInput input)
		{
			var query = _cacheService.GetAllCachedProducts().AsQueryable();
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
			var products = await _productService.FetchProductsFromCategory(category);
			if (products != null)
			{
				foreach (var item in products)
				{
					if (AllowedCategory(item))
					{
						_cacheService.CacheProduct(item);
					}
				}
			}
		}

		private async Task<Product?> FetchAndCacheProduct(int id)
		{
			var product = await _productService.FetchProduct(id);
			if (product != null)
			{
				_cacheService.CacheProduct(product);
				return product;
			}
			return null;
		}

		private bool AllowedCategory(Product item)
		{
			var allowedCategories = new[] { Category.Electronics, Category.Jewelery };
			return allowedCategories.Contains(item.Category);
		}

		private IQueryable<Product> AddOrderToQuery(IQueryable<Product> query, string orderProperty, SearchOrder order)
		{
			return (orderProperty.ToLower(), order) switch
			{
				("title", SearchOrder.Asc) => query.OrderBy(x => x.Title).ThenBy(x => x.ProductId),
				("title", SearchOrder.Desc) => query.OrderByDescending(x => x.Title).ThenByDescending(x => x.ProductId),
				("description", SearchOrder.Asc) => query.OrderBy(x => x.Description).ThenBy(x => x.ProductId),
				("description", SearchOrder.Desc) => query.OrderByDescending(x => x.Description).ThenByDescending(x => x.ProductId),
				_ => query.OrderBy(x => x.Title).ThenBy(x => x.ProductId)
			};
		}
		private bool ShouldExcludeDeletedProduct(int productId, bool includeDeleted)
		{
			if (includeDeleted)
			{
				return false;
			}
			return _cacheService.IsProductDeleted(productId);
		}
	}
}
