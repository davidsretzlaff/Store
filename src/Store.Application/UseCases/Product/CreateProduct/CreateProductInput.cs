using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Product.CreateProduct
{
	public record CreateProductInput
	(
		int Id,
		string Title,
		decimal Price,
		string Description,
		string Category,
		string Cnpj
	) : IRequest<ProductOutput>;
}
