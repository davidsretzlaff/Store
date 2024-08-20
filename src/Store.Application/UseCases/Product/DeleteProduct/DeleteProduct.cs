using Store.Application.Common.Exceptions;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;

namespace Store.Application.UseCases.Product.DeleteProduct
{
    internal class DeleteProduct : IDeleteProduct
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;

		public DeleteProduct(
			IProductRepository productRepository,
			IUserValidation userValidation,
			IUnitOfWork unitOfWork
		)
		{ 
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
			_userValidation = userValidation;
		}
		public async Task<ProductOutput> Handle(DeleteProductInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.Cnpj, cancellationToken);
			var product = await _productRepository.Get(input.Id, false, cancellationToken);
			RelatedAggregateException.ThrowIfNull(product, $"Product with ID '{input.Id}' not found");
			await _productRepository.Delete(product, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return ProductOutput.FromProduct(product);
		}
	}
}
