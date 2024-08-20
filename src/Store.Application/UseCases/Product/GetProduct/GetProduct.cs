using Store.Application.Common.Exceptions;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;

namespace Store.Application.UseCases.Product.GetProduct
{
    public class GetProduct : IGetProduct
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;

		public GetProduct(
			IProductRepository productRepository,
			IUserValidation userValidation,
			IUnitOfWork unitOfWork
		)
		{
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
			_userValidation = userValidation;
		}
		public async Task<ProductOutput> Handle(GetProductInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.User, cancellationToken);
			var product = await _productRepository.Get(input.Id, false, cancellationToken);
			RelatedAggregateException.ThrowIfNull(product, $"Product '{input.Id}' not found.");
			return ProductOutput.FromProduct(product!);
		}
	}
}
