using MediatR;

namespace Store.Application.UseCases.Product.ListProducts
{
	public interface IListProducts : IRequestHandler<ListProductsInput, ListProductsOutput>
	{
	}
}
