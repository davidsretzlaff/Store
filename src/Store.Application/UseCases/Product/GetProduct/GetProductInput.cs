using MediatR;
using Store.Application.UseCases.Order.Common;

namespace Store.Application.UseCases.Product.GetProduct
{
	public record GetProductInput
	(
		int Id,
		string? User
	) : IRequest<ProductOutput>;
}
