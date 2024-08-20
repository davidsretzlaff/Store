using Store.Application.Common.UserValidation;
using Store.Application.UseCases.Order.Common;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;

namespace Store.Application.UseCases.Product.ListProducts
{
    public class ListProducts : IListProducts
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;

		public ListProducts
		(
			IProductRepository productRepository, 
			IUnitOfWork unitOfWork, 
			IUserValidation userValidation
		)
		{
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
			_userValidation = userValidation;
		}
		public async Task<ListProductsOutput> Handle(ListProductsInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.User, cancellationToken);
			var searchOutput = await _productRepository.Search(
				new Domain.SeedWork.Searchable.SearchInput(
						input.Page,
						input.PerPage,
						input.Search,
						input.OrderBy,
						input.Order,
						input.User
					),
					cancellationToken
				);

			return new ListProductsOutput(
				searchOutput.CurrentPage,
				searchOutput.PerPage,
				searchOutput.Total,
				searchOutput.Items.Select(ProductOutput.FromProduct).ToList()
				);
		}
	}
}
