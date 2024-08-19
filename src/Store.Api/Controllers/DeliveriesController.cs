using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Models.CreateDelivery;
using Store.Api.Models.CreateOrder;
using Store.Application.Common.Models.Response;
using Store.Application.UseCases.Delivery.Common;
using Store.Application.UseCases.Delivery.CompleteDelivery;
using Store.Application.UseCases.Delivery.CreateDelivery;
using Store.Application.UseCases.Delivery.StartDelivery;
using Store.Application.UseCases.Order.ApproveOrder;
using Store.Application.UseCases.Order.Common;
using Store.Application.UseCases.Order.CreateOrder;
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
		public async Task<IActionResult> Create(
			[FromBody] ApiCreateDeliveryInput input,
			CancellationToken cancellationToken
		)
		{
			var companyRegisterNumber = User.Claims.FirstOrDefault(c => c.Type == "CompanyRegisterNumber")?.Value;
			var CreateOrderInputApplication = input.ToInput(companyRegisterNumber);
			
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
		public async Task<IActionResult> Start(
			string id,
			CancellationToken cancellationToken
		)
		{
			var companyRegisterNumber = User.Claims.FirstOrDefault(c => c.Type == "CompanyRegisterNumber")?.Value;
			var output = await _mediator.Send(new StartDeliveryInput(id, companyRegisterNumber), cancellationToken);
			return Ok(new Response<DeliveryOutput>(output));
		}

		[HttpPut("{id}/Complete")]
		[ProducesResponseType(typeof(Response<DeliveryOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Complete(
			string id,
			CancellationToken cancellationToken
		)
		{
			var companyRegisterNumber = User.Claims.FirstOrDefault(c => c.Type == "CompanyRegisterNumber")?.Value;
			var output = await _mediator.Send(new CompleteDeliveryInput(id, companyRegisterNumber), cancellationToken);
			return Ok(new Response<DeliveryOutput>(output));
		}
	}
}
