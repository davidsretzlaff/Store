using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Models.CreateOrder;
using Store.Application.Common.Models.Response;
using Store.Application.UseCases.Order.ApproveOrder;
using Store.Application.UseCases.Order.CancelOrder;
using Store.Application.UseCases.Order.Common;
using Store.Application.UseCases.Order.CreateOrder;
using Store.Application.UseCases.Order.ListOrders;
using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Interface.Infra.Adapters;

namespace Store.Api.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class OrdersController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IJwtUtils _jwtUtils;

		public OrdersController(IMediator mediator, IJwtUtils jwtUtils)
		{
			_mediator = mediator;
			_jwtUtils = jwtUtils;
		}

		[HttpPost("Create")]
		[ProducesResponseType(typeof(Response<OrderOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Create(
			[FromBody] ApiCreateOrderInput input,
		  CancellationToken cancellationToken
		)
		{
			var Cnpj = User.Claims.FirstOrDefault(c => c.Type == "Cnpj")?.Value;
			var CreateOrderInputApplication = new CreateOrderInput(Cnpj!, input.CustomerName, input.CustomerDocument, input.ProductIds);
			var output = await _mediator.Send(CreateOrderInputApplication, cancellationToken);
			return CreatedAtAction(
				nameof(Create),
				new { output.Id },
				new Response<OrderOutput>(output)
			);
		}

		[HttpPut("{id}/Approve")]
		[ProducesResponseType(typeof(Response<UpdateOrderOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Approve(
		  string id,
		  CancellationToken cancellationToken
		)
		{
			var Cnpj = User.Claims.FirstOrDefault(c => c.Type == "Cnpj")?.Value;
			var output = await _mediator.Send(new ApproveOrderInput(id, Cnpj), cancellationToken);
			return Ok(new Response<UpdateOrderOutput>(output));
		}

		[HttpPut("{id}/Cancel")]
		[ProducesResponseType(typeof(Response<UpdateOrderOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Cancel(
		  string id,
		  CancellationToken cancellationToken
		)
		{
			var Cnpj = User.Claims.FirstOrDefault(c => c.Type == "Cnpj")?.Value;
			var output = await _mediator.Send(new CancelOrderInput(id, Cnpj!), cancellationToken);
			return Ok(new Response<UpdateOrderOutput>(output));
		}

		[HttpGet("List")]
		[ProducesResponseType(typeof(ListOrdersOutput), StatusCodes.Status200OK)]
		public async Task<IActionResult> List(
		CancellationToken cancellationToken,
			[FromQuery] int? Page = null,
			[FromQuery] int? PerPage = null,
			[FromQuery] string? Search = null,
			[FromQuery] string? OrderBy = null,
			[FromQuery] SearchOrder? Order = null
		)
		{
			var Cnpj = User.Claims.FirstOrDefault(c => c.Type == "Cnpj")?.Value;
			var input = new ListOrdersInput(Cnpj!);
			if (Page is not null) input.Page = Page.Value;
			if (PerPage is not null) input.PerPage = PerPage.Value;
			if (!String.IsNullOrWhiteSpace(Search)) input.Search = Search;
			if (!String.IsNullOrWhiteSpace(OrderBy)) input.OrderBy = OrderBy;
			if (Order is not null) input.Order = Order.Value;
			

			var output = await _mediator.Send(input, cancellationToken);
			return Ok(new ResponseList<OrderOutput>(output));
		}
	}
}
