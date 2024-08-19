
using Store.Application.UseCases.Order.Common;
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
			var order = await _orderRepository.Search(input.ToSearchInput(), cancellationToken);

			IReadOnlyList<DomainEntity.Product>? products = null;
			var relatedProductsIds = order.Items.SelectMany(order => order.GetProductsIds()).ToList();
			if (relatedProductsIds is not null && relatedProductsIds.Count > 0)
			{
				products = await _productRepository.GetListByIds(relatedProductsIds, cancellationToken);
			}
			
			return new ListOrdersOutput(
				order.CurrentPage,
				order.PerPage,
				order.Total,
				order.Items.Select(item => OrderOutput.FromOrder(item, products)).ToList()
			);
		}
	}
}
