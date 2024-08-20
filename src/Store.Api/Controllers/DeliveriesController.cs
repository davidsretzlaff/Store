using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Models.CreateDelivery;
using Store.Api.Models.CreateOrder;
using Store.Application.Common.Models.Response;
using Store.Application.UseCases.Delivery.Common;
using Store.Application.UseCases.Delivery.CompleteDelivery;
using Store.Application.UseCases.Delivery.CreateDelivery;
using Store.Application.UseCases.Delivery.ListDeliveries;
using Store.Application.UseCases.Delivery.StartDelivery;
using Store.Application.UseCases.Order.ApproveOrder;
using Store.Application.UseCases.Order.Common;
using Store.Application.UseCases.Order.CreateOrder;
using Store.Domain.Enum;
using Store.Domain.Extensions;
using Store.Domain.Interface.Infra.Adapters;

namespace Store.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DeliveriesController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IJwtUtils _jwtUtils;

		public DeliveriesController(IMediator mediator, IJwtUtils jwtUtils)
		{
			_mediator = mediator;
			_jwtUtils = jwtUtils;
		}

		[HttpPost("Create")]
		[ProducesResponseType(typeof(Response<DeliveryOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Create
		(
			[FromBody] ApiCreateDeliveryInput input,
			CancellationToken cancellationToken
		)
		{
			var Cnpj = User.Claims.FirstOrDefault(c => c.Type == "Cnpj")?.Value;
			var CreateOrderInputApplication = input.ToInput(Cnpj);
			
			var output = await _mediator.Send(CreateOrderInputApplication, cancellationToken);
			return CreatedAtAction(
				nameof(Create),
				new { output.OrderId },
				new Response<DeliveryOutput>(output)
			);
		}

		[HttpPut("{id}/Start")]
		[ProducesResponseType(typeof(Response<DeliveryOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Start
		(
			string id,
			CancellationToken cancellationToken
		)
		{
			var Cnpj = User.Claims.FirstOrDefault(c => c.Type == "Cnpj")?.Value;
			var output = await _mediator.Send(new StartDeliveryInput(id, Cnpj), cancellationToken);
			return Ok(new Response<DeliveryOutput>(output));
		}

		[HttpPut("{id}/Complete")]
		[ProducesResponseType(typeof(Response<DeliveryOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Complete
		(
			string id,
			CancellationToken cancellationToken
		)
		{
			var Cnpj = User.Claims.FirstOrDefault(c => c.Type == "Cnpj")?.Value;
			var output = await _mediator.Send(new CompleteDeliveryInput(id, Cnpj), cancellationToken);
			return Ok(new Response<DeliveryOutput>(output));
		}

		[HttpGet("List")]
		[ProducesResponseType(typeof(ListDeliveriesInput), StatusCodes.Status200OK)]
		public async Task<IActionResult> List
		(
			CancellationToken cancellationToken,
			[FromQuery] int? Page = null,
			[FromQuery] int? PerPage = null,
			[FromQuery] string? Search = null,
			[FromQuery] string? OrderBy = null,
			[FromQuery] SearchOrder? Order = null
		)
		{
			var Cnpj = User.Claims.FirstOrDefault(c => c.Type == "Cnpj")?.Value;
			var input = new ListDeliveriesInput(Cnpj);
			if (Page is not null) input.Page = Page.Value;
			if (PerPage is not null) input.PerPage = PerPage.Value;
			if (!String.IsNullOrWhiteSpace(Search)) input.Search = Search;
			if (!String.IsNullOrWhiteSpace(OrderBy)) input.OrderBy = OrderBy;
			if (Order is not null) input.Order = Order.Value;

			var output = await _mediator.Send(input, cancellationToken);
			return Ok(new ResponseList<DeliveryOutput>(output));
		}
	}
}
