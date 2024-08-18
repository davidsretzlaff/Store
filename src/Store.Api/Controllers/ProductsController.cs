using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Common.Models.Response;
using Store.Application.UseCases.Order.Common;
using Store.Application.UseCases.Product.ListProducts;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.ListUsers;
using Store.Domain.Enum;

namespace Store.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ProductsController(IMediator mediator) => _mediator = mediator;

		[HttpGet]
		[ProducesResponseType(typeof(ListProductsInput), StatusCodes.Status200OK)]
		public async Task<IActionResult> List(
		CancellationToken cancellationToken,
			[FromQuery] int? Page = null,
			[FromQuery] int? PerPage = null,
			[FromQuery] string? Search = null,
			[FromQuery] string? OrderBy = null,
			[FromQuery] SearchOrder? Order = null
		)
		{
			var input = new ListProductsInput();
			if (Page is not null) input.Page = Page.Value;
			if (PerPage is not null) input.PerPage = PerPage.Value;
			if (!String.IsNullOrWhiteSpace(Search)) input.Search = Search;
			if (!String.IsNullOrWhiteSpace(OrderBy)) input.OrderBy = OrderBy;
			if (Order is not null) input.Order = Order.Value;

			var output = await _mediator.Send(input, cancellationToken);
			return Ok(new ResponseList<ProductOutput>(output));
		}
	}
}
