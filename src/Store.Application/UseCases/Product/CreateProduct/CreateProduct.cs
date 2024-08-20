using Store.Application.Common.Exceptions;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Extensions;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Product.CreateProduct
{
    internal class CreateProduct : ICreateProduct
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;

		public CreateProduct(
			IProductRepository productRepository,
			IUserValidation userValidation,
			IUnitOfWork unitOfWork
		)
		{
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
			_userValidation = userValidation;
		}
		public async Task<ProductOutput> Handle(CreateProductInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.User, cancellationToken);
			var existingProduct = await _productRepository.Get(input.Id, false, cancellationToken);
			DuplicateException.ThrowIfHasValue(existingProduct, $"A product with ID '{input.Id}' already exists. Please use a unique ID to avoid duplication");
			
			var product = new DomainEntity.Product(
				input.Id,
				input.Title,
				input.Description,
				input.Price,
				input.Category.ToCategory()
			);
			await _productRepository.Insert(product, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return ProductOutput.FromProduct(product);
		}
	}
}
