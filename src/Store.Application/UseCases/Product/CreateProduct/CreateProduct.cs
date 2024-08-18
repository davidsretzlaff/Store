using Store.Application.Common.Interface;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Extensions;
using Store.Domain.Repository;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Product.CreateProduct
{
	internal class CreateProduct : ICreateProduct
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CreateProduct(IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
		}
		public async Task<ProductOutput> Handle(CreateProductInput input, CancellationToken cancellationToken)
		{
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
