
using Store.Application.UseCases.Order.Common;
using Store.Domain.Entity;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.ListOrders
{
    public class ListOrders : IListOrders
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;
		public ListOrders(
			IOrderRepository orderRepository, 
			IProductRepository productRepository, 
			IUserValidation userValidation,  
			IUnitOfWork unitOfWork
		)
		{
			_orderRepository = orderRepository;
			_productRepository = productRepository;
			_unitOfWork = unitOfWork;
			_userValidation = userValidation;
		}
		public async Task<ListOrdersOutput> Handle(ListOrdersInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.CompanyRegisterNumber, cancellationToken);

			var orders = await _orderRepository.Search(input.ToSearchInput(), cancellationToken);
            
			foreach (var order in orders.Items)
            {
				var relatedProductsIds = orders.Items
					.SelectMany(item => item.Items)
					.Select(item => item.ProductId)
					.Distinct()
					.ToList();


				var products = await _productRepository.GetListByIds(relatedProductsIds, true, cancellationToken);
				
				foreach (var product in products)
				{
					var item = order.Items.Where(item => item.ProductId == product.Id).FirstOrDefault();
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
