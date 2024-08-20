using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Product.DeleteProduct
{
	public record DeleteProductInput
	(
		int Id,
		string Cnpj
	) : IRequest<ProductOutput>;
}
