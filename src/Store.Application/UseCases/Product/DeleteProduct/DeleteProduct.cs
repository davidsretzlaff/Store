using Store.Application.Common.Exceptions;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Interface.Repository;

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
			NotFoundException.ThrowIfNull(product, $"Product with ID '{input.Id}' not found");
			await _productRepository.Delete(product, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return ProductOutput.FromProduct(product);
		}
	}
}
