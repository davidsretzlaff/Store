using Microsoft.EntityFrameworkCore;
using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.Interface.Infra.Repository;
using Store.Domain.SeedWork.Searchable;
using Store.Domain.ValueObject;
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
							order.OrderId,
							product.ProductId,
							product.Quantity
						));
				await _ordersProducts.AddRangeAsync(relation);
			}
		}

		public async Task<Order?> Get(string id, CancellationToken cancellationToken)
		{
			var order = await _orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == id, cancellationToken);
			return order;
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
					x.OrderId.ToLower().Contains(searchToLower) ||
					x.Status.ToOrderStatusString().ToLower().Contains(searchToLower) ||
					x.CustomerDocument.Value.ToLower().StartsWith(searchToLower) ||
					x.CustomerName.ToLower().Contains(searchToLower)
				);
			}
			var userCnpj = Cnpj.RemoveNonDigits(input.User);
			query = query.Where(x => 
				x.CompanyIdentificationNumber.Value.Equals(userCnpj));

			var total = await query.CountAsync();
			var items = await query
				.Skip(toSkip)
				.Take(input.PerPage)
				.ToListAsync();

			var productIds = items.Select(order => order.OrderId).ToList();
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
				var order = items.Find(o => o.OrderId == relationGroup.Key);
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
				("createddate", SearchOrder.Asc) => query.OrderBy(x => x.CreatedDate).ThenBy(x => x.OrderId),
				("createddate", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.OrderId),
				("status", SearchOrder.Asc) => query.OrderBy(x => x.Status.ToOrderStatusString()).ThenBy(x => x.OrderId),
				("status", SearchOrder.Desc) => query.OrderByDescending(x => x.Status.ToOrderStatusString()).ThenByDescending(x => x.OrderId),
				("customername", SearchOrder.Asc) => query.OrderBy(x => x.CustomerName).ThenBy(x => x.OrderId),
				("customername", SearchOrder.Desc) => query.OrderByDescending(x => x.CustomerName).ThenByDescending(x => x.OrderId),
				_ => query.OrderBy(x => x.CreatedDate).ThenBy(x => x.OrderId)
			};
			return orderedQuery;
		}

		public async Task Update(Order order, CancellationToken cancellationToken)
		{
			await Task.FromResult(_orders.Update(order));
		}
	}
}
