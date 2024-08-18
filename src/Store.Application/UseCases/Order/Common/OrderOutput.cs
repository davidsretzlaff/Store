using Store.Domain.Enum;
using Store.Domain.Extensions;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.Common
{
	public record OrderOutput
	(
		string Id,
		string CompanyRegisterNumber,
		DateTime CreatedData,
		string CustomerName,
		string CustomerDocument,
		string Status,
		int ItemsQuantity,
		string Total,
		List<ProductOutput> Products
	)
	{
		public static OrderOutput FromOrder(DomainEntity.Order order)
		{
			return new OrderOutput(
				order.Id,
				order.CompanyRegisterNumber,
				order.CreatedData,
				order.CustomerName,
				order.CustomerDocument,
				order.Status.ToOrderStringStatus(),
				order.GetTotalProducts(),
				order.GetTotal(),
				order.Products.Select(ProductOutput.FromProduct).ToList()
			);
		}
	}
}
