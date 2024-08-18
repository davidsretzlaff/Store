using Store.Application.Common.Interface;
using Store.Application.Common.Models.Response;
using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.Interface;
using Store.Domain.Interface.Repository;
using Store.Domain.SeedWork.Searchable;
using Store.Infra.Adapters.ExternalCatalog.Models;

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

			_cacheService.MarkProductAsDeleted(product.Id);
			_cacheService.RemoveProductFromCache(product.Id);
			return Task.CompletedTask;
		}

		public async Task<Product?> Get(int id, CancellationToken cancellationToken)
		{
			if (_cacheService.IsProductDeleted(id))
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
			if (products is not null)
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
			if (product is not null)
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
				("title", SearchOrder.Asc) => query.OrderBy(x => x.Title).ThenBy(x => x.Id),
				("title", SearchOrder.Desc) => query.OrderByDescending(x => x.Title).ThenByDescending(x => x.Id),
				("description", SearchOrder.Asc) => query.OrderBy(x => x.Description).ThenBy(x => x.Id),
				("description", SearchOrder.Desc) => query.OrderByDescending(x => x.Description).ThenByDescending(x => x.Id),
				_ => query.OrderBy(x => x.Title).ThenBy(x => x.Id)
			};
		}
	}
}
