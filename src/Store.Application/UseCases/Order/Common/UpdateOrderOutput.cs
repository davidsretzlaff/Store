﻿using Store.Domain.Extensions;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.Common
{
	public record UpdateOrderOutput
	(
		string Id,
		string Cnpj,
		string CreatedData,
		string CustomerName,
		string CustomerDocument,
		string Status
	)
	{
		public static UpdateOrderOutput FromOrder(DomainEntity.Order order)
		{
			return new UpdateOrderOutput(
				order.OrderId,
				order.CompanyIdentificationNumber.Value,
				order.FormattedDate(),
				order.CustomerName,
				order.CustomerDocument.Value,
				order.Status.ToOrderStatusString()
			);
		}
	}
}
