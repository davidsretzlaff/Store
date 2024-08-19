using Store.Domain.Enum;
using Store.Domain.Extensions;
using System.Collections.Generic;
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
		string Total,
		int ItemCount,
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
				order.Status.ToOrderStatusString(),
				order.GetTotalAsCurrency(),
				order.GetProductCount(),
				order.Products.Select(ProductOutput.FromProduct).ToList()
			);
		}

		public static OrderOutput FromOrder(
			DomainEntity.Order order,
			IReadOnlyList<Domain.Entity.Product> products
		)
		{
			products.ToList().ForEach( p => order.AddProduct(p.Id, p.Title,p.Description, p.Price, p.Category));

			return new OrderOutput(
				order.Id,
				order.CompanyRegisterNumber,
				order.CreatedData,
				order.CustomerName,
				order.CustomerDocument,
				order.Status.ToOrderStatusString(),
				order.GetTotalAsCurrency(),
				order.GetProductCount(),
				order.Products.Select(ProductOutput.FromProduct).ToList()
			);
		}
	}
}