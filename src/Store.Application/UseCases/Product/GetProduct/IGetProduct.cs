using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Product.GetProduct
{
	public interface IGetProduct : IRequestHandler<GetProductInput, ProductOutput>
	{
	}
}
