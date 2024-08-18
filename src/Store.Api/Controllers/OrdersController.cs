using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Models.CreateOrder;
using Store.Application.Common.Interface;
using Store.Application.Common.Models.Response;
using Store.Application.UseCases.Order.Common;
using Store.Application.UseCases.Order.CreateOrder;
using Store.Infra.Adapters.Identity;

namespace Store.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrdersController : ControllerBase
	{
		private readonly IMediator _mediator;

		public OrdersController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[ProducesResponseType(typeof(Response<OrderOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		[Authorize]
		public async Task<IActionResult> Create(
		  [FromBody] ApiCreateOrderInput input,
		  CancellationToken cancellationToken
		)
		{
			var companyRegisterNumber = User.Claims.FirstOrDefault(c => c.Type == "CompanyRegisterNumber")?.Value;
			var CreateOrderInputApplication = new CreateOrderInput(companyRegisterNumber, input.CustomerName, input.CustomerDocument, input.ProductIds);
			var output = await _mediator.Send(CreateOrderInputApplication, cancellationToken);
			return CreatedAtAction(
				nameof(Create),
				new { output.Id },
				new Response<OrderOutput>(output)
			);
		}
	}
}
