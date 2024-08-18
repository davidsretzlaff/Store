using Microsoft.EntityFrameworkCore;
using Store.Domain.Entity;
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
			if (order.Products.Count > 0)
			{
				var relation = order.Products
					.Select(product => new OrdersProducts(
							order.Id,
							product.Id
						));
				await _ordersProducts.AddRangeAsync(relation);
			}
		}

		public Task Delete(Order aggregate, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<Order> Get(Guid id, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}


		public Task<SearchOutput<Order>> Search(SearchInput input, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task Update(Order aggregate, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
