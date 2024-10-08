﻿using Store.Domain.Enum;
using Store.Domain.Extensions;
using System.Collections.Generic;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.Common
{
	public record OrderOutput
	(
		string Id,
		string Cnpj,
		string CreatedDate,
		string CustomerName,
		string CustomerDocument,
		string Status,
		string Total,
		int ItemCount,
		List<ItemOutput> Items
	)
	{
		public static OrderOutput FromOrder(DomainEntity.Order order)
		{
			return new OrderOutput(
				order.OrderId,
				order.CompanyIdentificationNumber.Value,
				order.FormattedDate(),
				order.CustomerName,
				order.CustomerDocument.Value,
				order.Status.ToOrderStatusString(),
				order.GetTotalAsCurrency(),
				order.GetProductCount(),
				order.Items.Select(ItemOutput.FromProduct).ToList()
			);
		}
	}
}