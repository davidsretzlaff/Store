using Store.Application.Common.Exceptions;
using Store.Application.Common.Interface;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Repository;

namespace Store.Application.UseCases.Product.GetProduct
{
	public class GetProduct : IGetProduct
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;

		public GetProduct(IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
		}
		public async Task<ProductOutput> Handle(GetProductInput input, CancellationToken cancellationToken)
		{
			var product = await _productRepository.Get(input.Id, cancellationToken);
			NotFoundException.ThrowIfNull(product, $"Product '{input.Id}' not found.");
			return ProductOutput.FromProduct(product);
		}
	}
}
