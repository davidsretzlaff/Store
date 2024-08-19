using Store.Application.Common.Exceptions;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Entity;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;
using System.Security.Cryptography.X509Certificates;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.CreateOrder
{
    public class CreateOrder : ICreateOrder
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;

		public CreateOrder(
			IOrderRepository orderRepository,
			IProductRepository productRepository,
			IUnitOfWork unitOfWork,
			IUserValidation userValidation
		)
		{
			_orderRepository = orderRepository;
			_productRepository = productRepository;
			_unitOfWork = unitOfWork;
			_userValidation = userValidation;
		}

		public async Task<OrderOutput> Handle(CreateOrderInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.CompanyRegisterNumber, cancellationToken);
			var products = await GetValidProducts(input.ProductIds, cancellationToken);
			var order = CreateOrderDomain(input, products);
			order.Validate();

			await _orderRepository.Insert(order, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);

			return OrderOutput.FromOrder(order);
		}

		
		private async Task<List<DomainEntity.Product>> GetValidProducts(List<int> productIds, CancellationToken cancellationToken)
		{
			RelatedAggregateException.ThrowIfNull(productIds, "Product IDs cannot be null or empty");

			var products = await GetProductsAsync(productIds, cancellationToken);

			return products;
		}
		private async Task<List<DomainEntity.Product>> GetProductsAsync(List<int> productIds, CancellationToken cancellationToken)
		{
			var productTasks = productIds.Select(id => _productRepository.Get(id,false, cancellationToken));
			var productsArray = await Task.WhenAll(productTasks);
			ValidateProducts(productsArray, productIds);
			return productsArray.ToList()!;
		}
		
		private void ValidateProducts(DomainEntity.Product?[] productsArray, List<int> productIds)
		{
			var invalidProductIds = productIds
				.Where((id, index) => productsArray[index] == null)
				.ToList();

			if (invalidProductIds.Any())
			{
				var invalidIdsString = string.Join(", ", invalidProductIds);
				RelatedAggregateException.Throw($"Products with IDs '{invalidIdsString}' not found.");
			}
		}

		private DomainEntity.Order CreateOrderDomain(CreateOrderInput input, List<DomainEntity.Product> products)
		{
			var order = new DomainEntity.Order(input.CompanyRegisterNumber, input.CustomerName, input.CustomerDocument);
			var productGroups = products
			  .GroupBy(p => p.Id)
			  .Select(g => new
			  {
				  Product = g.First(),
				  Count = g.Count()   
			  })
			  .ToList();
			foreach (var productGroup in productGroups)
			{
				order.AddItem(productGroup.Product.Id, productGroup.Count, productGroup.Product);
			}
			return order;
		}
	}
}
