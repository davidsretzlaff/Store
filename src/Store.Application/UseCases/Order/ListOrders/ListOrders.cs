
using Store.Application.UseCases.Order.Common;
using Store.Domain.Entity;
using Store.Domain.Interface.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.ListOrders
{
	public class ListOrders : IListOrders
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		public ListOrders(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			_orderRepository = orderRepository;
			_productRepository = productRepository;
			_unitOfWork = unitOfWork;
		}
		public async Task<ListOrdersOutput> Handle(ListOrdersInput input, CancellationToken cancellationToken)
		{
			var orders = await _orderRepository.Search(input.ToSearchInput(), cancellationToken);

			//IReadOnlyList<DomainEntity.Product>? products = null;
            //var relatedProductsIds = orders.Items.SelectMany(orders => orders.Items).ToList();
            //if (relatedProductsIds is not null && relatedProductsIds.Count > 0)
            //{
            //	products = await _productRepository.GetListByIds(relatedProductsIds.ForEach(i => i.ProductId).ToList(), cancellationToken);
            //}

            foreach (var order in orders.Items)
            {
				var relatedProductsIds = orders.Items
					.SelectMany(item => item.Items)
					.Select(item => item.ProductId)
					.Distinct() // Remove duplicates if needed
					.ToList();


				var products = await _productRepository.GetListByIds(relatedProductsIds, cancellationToken);
				
				foreach (var product in products)
				{
					var item = order.Items.First(item => item.ProductId == product.Id);
					if (item is not null)
					{
						item.addProduct(product);
					}
				
				}
			}

            return new ListOrdersOutput(
				orders.CurrentPage,
				orders.PerPage,
				orders.Total,
				orders.Items.Select(item => OrderOutput.FromOrder(item)).ToList()
			);
		}
	}
}
