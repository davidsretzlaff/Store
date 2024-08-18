using Store.Application.UseCases.Order.CreateOrder;

namespace Store.Api.Models.CreateOrder
{
	public record ApiCreateOrderInput(
		string CustomerName,
		string CustomerDocument,
		List<int> ProductIds
	);
}
