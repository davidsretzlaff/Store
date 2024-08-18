using Store.Application.Common.Interface;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.CreateOrder
{
	public class CreateOrder : ICreateOrder
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CreateOrder(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			_orderRepository = orderRepository;
			_productRepository = productRepository;
			_unitOfWork = unitOfWork;
		}
		public async Task<OrderOutput> Handle(CreateOrderInput input, CancellationToken cancellationToken)
		{
			List<DomainEntity.Product> products = new List<DomainEntity.Product>();
			foreach (var item in input.ProductIds)
			{
				var product = await _productRepository.Get(item, cancellationToken);
				products.Add(product);
			}
			var order = new DomainEntity.Order(input.CompanyRegisterNumber, input.CustomerName, input.CustomerDocument);
			products.ForEach(order.AddProduct);
			order.Validate();
			return OrderOutput.FromOrder(order);
		}
	}
}
