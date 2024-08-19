using Microsoft.EntityFrameworkCore;
using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.Interface.Repository;
using Store.Domain.SeedWork.Searchable;
using Store.Infra.Data.EF.Models;

namespace Store.Infra.Data.EF.Repositories
{
    public class OrderRepository : IOrderRepository
	{
		private readonly StoreDbContext _context;
		private DbSet<Order> _orders => _context.Set<Order>();
		private DbSet<OrdersProducts> _ordersProducts => _context.Set<OrdersProducts>();
		public OrderRepository(StoreDbContext context) => _context = context;

		public async Task Insert(Order order, CancellationToken cancellationToken)
		{
			await _orders.AddAsync(order, cancellationToken);
			if (order.Items.Count > 0)
			{
				var relation = order.Items
					.Select(product => new OrdersProducts(
							order.Id,
							product.ProductId,
							product.Quantity
						));
				await _ordersProducts.AddRangeAsync(relation);
			}
		}

		public Task<Order> Get(Guid id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}


		public async Task<SearchOutput<Order>> Search(SearchInput input, CancellationToken cancellationToken)
		{
			var toSkip = (input.Page - 1) * input.PerPage;
			var query = _orders.AsNoTracking();
			query = AddOrderToQuery(query, input.OrderBy, input.Order);

			if (!string.IsNullOrWhiteSpace(input.Search))
			{
				var searchToLower = input.Search.ToLower();
				query = query.Where(x =>
					x.Status.ToOrderStatusString().Contains(searchToLower) ||
					x.CustomerDocument.ToLower().StartsWith(searchToLower) ||
					x.CustomerName.ToLower().Contains(searchToLower)
				);
			}

			var total = await query.CountAsync();
			var items = await query
				.Skip(toSkip)
				.Take(input.PerPage)
				.ToListAsync();

			var productIds = items.Select(order => order.Id).ToList();
			await AddProductsToOrder(items, productIds);

			return new SearchOutput<Order>(input.Page, input.PerPage, total, items);
		}

		private async Task AddProductsToOrder(List<Order> items, List<string> ordersId)
		{
			// Fetch the product relations based on the provided order IDs
			var productsRelations = await _ordersProducts
				.Where(relation => ordersId.Contains(relation.OrderId))
				.ToListAsync();

			// Group the relations by OrderId
			var relationsWithProductsByOrderId = productsRelations
				.GroupBy(x => x.OrderId)
				.ToList();

			// Iterate over each group of relations
			foreach (var relationGroup in relationsWithProductsByOrderId)
			{
				// Find the corresponding order in the provided list
				var order = items.Find(o => o.Id == relationGroup.Key);
				if (order == null) continue;

				// Iterate over each relation in the group and add items to the order
				foreach (var relation in relationGroup)
				{
					// Add each product to the order with the specified quantity
					order.AddItem(relation.ProductId, relation.Quantity);
				}
			}
		}

		private IQueryable<Order> AddOrderToQuery(IQueryable<Order> query, string orderProperty, SearchOrder order)
		{
			var orderedQuery = (orderProperty.ToLower(), order) switch
			{
				("createddata", SearchOrder.Asc) => query.OrderBy(x => x.CreatedData).ThenBy(x => x.Id),
				("createddata", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedData).ThenByDescending(x => x.Id),
				("status", SearchOrder.Asc) => query.OrderBy(x => x.Status).ThenBy(x => x.Id),
				("status", SearchOrder.Desc) => query.OrderByDescending(x => x.Status).ThenByDescending(x => x.Id),
				("customername", SearchOrder.Asc) => query.OrderBy(x => x.CustomerName).ThenBy(x => x.Id),
				("customername", SearchOrder.Desc) => query.OrderByDescending(x => x.CustomerName).ThenByDescending(x => x.Id),
				_ => query.OrderBy(x => x.CreatedData).ThenBy(x => x.Id)
			};
			return orderedQuery;
		}
	}
}
