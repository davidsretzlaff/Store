using Store.Application.UseCases.Order.Common;
using Store.Domain.Extensions;
using Store.Domain.Interface.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Product.DeleteProduct
{
    internal class DeleteProduct : IDeleteProduct
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;

		public DeleteProduct(IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
		}
		public async Task<ProductOutput> Handle(DeleteProductInput input, CancellationToken cancellationToken)
		{
			var product = await _productRepository.Get(input.Id, cancellationToken);
			await _productRepository.Delete(product, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return ProductOutput.FromProduct(product);
		}
	}
}
