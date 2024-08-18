﻿using Store.Domain.Extensions;
using DomainEntity = Store.Domain.Entity;

namespace Store.Application.UseCases.Order.Common
{
	public record ProductOutput
	(
		 int Id,
		 string Title,
		 string Description,
		 string Price,
		 string Category,
		 int Quantity
	)
	{
		public static ProductOutput FromProduct(DomainEntity.Product product)
		{
			return new ProductOutput(
				product.Id,
				product.Title,
				product.Description,
				$"R$ {product.Price}",
				product.Category.ToCategoryString(),
				product.Quantity
				);
		}
	}
}
