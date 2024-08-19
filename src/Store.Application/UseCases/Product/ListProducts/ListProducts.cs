using Store.Application.UseCases.Order.Common;
using Store.Domain.Interface.Infra.Repository;

namespace Store.Application.UseCases.Product.ListProducts
{
    public class ListProducts : IListProducts
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ListProducts(IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
		}
		public async Task<ListProductsOutput> Handle(ListProductsInput input, CancellationToken cancellationToken)
		{
			var searchOutput = await _productRepository.Search(
				new Domain.SeedWork.Searchable.SearchInput(
						input.Page,
						input.PerPage,
						input.Search,
						input.OrderBy,
						input.Order,
						string.Empty
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
