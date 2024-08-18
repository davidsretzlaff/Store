using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Common.Models.Response;
using Store.Application.UseCases.Order.Common;
using Store.Application.UseCases.Product.CreateProduct;
using Store.Application.UseCases.Product.DeleteProduct;
using Store.Application.UseCases.Product.GetProduct;
using Store.Application.UseCases.Product.ListProducts;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateUser;
using Store.Application.UseCases.User.GetUser;
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
		[Authorize]
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

		[ProducesResponseType(typeof(Response<UserOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		[Authorize]
		public async Task<IActionResult> Create(
			[FromBody] CreateProductInput input,
			CancellationToken cancellationToken
		)
		{
			var output = await _mediator.Send(input, cancellationToken);
			return CreatedAtAction(
				nameof(Create),
				new { output.Id },
				new Response<ProductOutput>(output)
			);
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(typeof(Response<ProductOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		[Authorize]
		public async Task<IActionResult> GetById(
			[FromRoute] int id,
			CancellationToken cancellationToken
		)
		{
			var output = await _mediator.Send(new GetProductInput(id), cancellationToken);
			return Ok(new Response<ProductOutput>(output));
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		[Authorize]
		public async Task<IActionResult> Delete(
		   [FromRoute] int id,
		   CancellationToken cancellationToken
	    )
		{
			await _mediator.Send(new DeleteProductInput(id), cancellationToken);
			return NoContent();
		}
	}
}
