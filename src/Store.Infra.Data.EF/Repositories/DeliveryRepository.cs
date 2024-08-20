using Microsoft.EntityFrameworkCore;
using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.Interface.Infra.Repository;
using Store.Domain.SeedWork.Searchable;
using Store.Domain.ValueObject;

namespace Store.Infra.Data.EF.Repositories
{
	public class DeliveryRepository : IDeliveryRepository
	{
		private readonly StoreDbContext _context;
		private DbSet<Delivery> _deliveries => _context.Set<Delivery>();
		public DeliveryRepository(StoreDbContext context) => _context = context;

		public async Task<Delivery?> Get(string orderId, CancellationToken cancellationToken)
		{
			var delivery = await _deliveries.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
			return delivery;
		}

		public async Task Update(Delivery delivery, CancellationToken cancellationToken)
		{
			await Task.FromResult(_deliveries.Update(delivery));
		}

		public async Task Insert(Delivery delivery, CancellationToken cancellationToken)
		{
			await _deliveries.AddAsync(delivery, cancellationToken);
		}

		public async Task<SearchOutput<Delivery>> Search(SearchInput input, CancellationToken cancellationToken)
		{
			var toSkip = (input.Page - 1) * input.PerPage;
			var query = _deliveries.AsNoTracking();
			query = AddOrderToQuery(query, input.OrderBy, input.Order);

			if (!string.IsNullOrWhiteSpace(input.Search))
			{
				var searchToLower = input.Search.ToLower();
				query = query.Where(x =>
					x.OrderId.ToLower().Contains(searchToLower) ||
					x.Status.ToDeliveryStatusString().ToLower().Contains(searchToLower)
				);
			}

			var userCnpj = Cnpj.RemoveNonDigits(input.User);
			query = query.Where(x => 
				x.CompanyIdentificationNumber.Value
				.Equals(userCnpj));

			var total = await query.CountAsync();
			var items = await query
				.Skip(toSkip)
				.Take(input.PerPage)
				.ToListAsync();

			return new SearchOutput<Delivery>(input.Page, input.PerPage, total, items);
		}

		private IQueryable<Delivery> AddOrderToQuery(IQueryable<Delivery> query, string orderProperty, SearchOrder order)
		{
			var orderedQuery = (orderProperty.ToLower(), order) switch
			{
				("orderid", SearchOrder.Asc) => query.OrderBy(x => x.OrderId).ThenBy(x => x.Id),
				("orderid", SearchOrder.Desc) => query.OrderByDescending(x => x.OrderId).ThenByDescending(x => x.Id),
				("status", SearchOrder.Asc) => query.OrderBy(x => x.Status.ToDeliveryStatusString()).ThenBy(x => x.Id),
				("status", SearchOrder.Desc) => query.OrderByDescending(x => x.Status.ToDeliveryStatusString()).ThenByDescending(x => x.Id),
				("delivereddate", SearchOrder.Asc) => query.OrderBy(x => x.DeliveredDate).ThenBy(x => x.Id),
				("delivereddate", SearchOrder.Desc) => query.OrderByDescending(x => x.DeliveredDate).ThenByDescending(x => x.Id),
				_ => query.OrderBy(x => x.OrderId).ThenBy(x => x.Id)
			};
			return orderedQuery;
		}
	}
}
