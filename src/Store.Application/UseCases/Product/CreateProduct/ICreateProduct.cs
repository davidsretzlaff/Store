using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Product.CreateProduct
{
	public interface ICreateProduct : IRequestHandler<CreateProductInput, ProductOutput>
	{
	}
}
