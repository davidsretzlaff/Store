using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Product.DeleteProduct
{
	public interface IDeleteProduct : IRequestHandler<DeleteProductInput, ProductOutput>
	{
	}
}
