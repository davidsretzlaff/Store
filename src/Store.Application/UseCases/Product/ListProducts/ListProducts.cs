using MediatR;
using Store.Application.Common.Models.PaginatedList;
using Store.Application.UseCases.Order.Common;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.ListUsers;
using Store.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
						input.Order
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
