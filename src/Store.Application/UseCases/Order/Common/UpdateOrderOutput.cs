using Store.Domain.Extensions;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.Common
{
	public record UpdateOrderOutput
	(
		string Id,
		string CompanyRegisterNumber,
		string CreatedData,
		string CustomerName,
		string CustomerDocument,
		string Status
	)
	{
		public static UpdateOrderOutput FromOrder(DomainEntity.Order order)
		{
			return new UpdateOrderOutput(
				order.Id,
				order.CompanyRegisterNumber,
				order.FormattedDate(),
				order.CustomerName,
				order.CustomerDocument,
				order.Status.ToOrderStatusString()
			);
		}
	}
}
