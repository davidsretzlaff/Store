using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Common.Models.Response;
using Store.Application.UseCases.User.ActivateUser;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateUser;
using Store.Application.UseCases.User.DeactiveUser;
using Store.Application.UseCases.User.GetUser;
using Store.Application.UseCases.User.ListUsers;
using Store.Domain.Enum;

namespace Store.Api.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IMediator _mediator;

		public UsersController(IMediator mediator) => _mediator = mediator;

		[HttpPost]
		[ProducesResponseType(typeof(Response<UserOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		[AllowAnonymous]
		public async Task<IActionResult> Create(
		  [FromBody] CreateUserInput input,
		  CancellationToken cancellationToken
		)
		{
			var output = await _mediator.Send(input, cancellationToken);
			return CreatedAtAction(
				nameof(Create),
				new { output.Id },
				new Response<UserOutput>(output)
			);
		}

		[HttpPut("{id:guid}/Activate")]
		[ProducesResponseType(typeof(Response<UserOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Activate(
		  Guid id,
		  CancellationToken cancellationToken
		)
		{
			var output = await _mediator.Send(new ActivateUserInput(id), cancellationToken);
			return Ok(new Response<UserOutput>(output));
		}

		[HttpPut("{id:guid}/Deactivate")]
		[ProducesResponseType(typeof(Response<UserOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Deactivate(
		  [FromRoute] Guid id,
		  CancellationToken cancellationToken
		)
		{
			var output = await _mediator.Send(new DeactivateUserInput(id), cancellationToken);
			return Ok(new Response<UserOutput>(output));
		}

		[HttpGet("{id:guid}")]
		[ProducesResponseType(typeof(Response<UserOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		[AllowAnonymous]
		public async Task<IActionResult> GetById(
		  [FromRoute] Guid id,
		  CancellationToken cancellationToken
		)
		{
			var output = await _mediator.Send(new GetUserInput(id), cancellationToken);
			return Ok(new Response<UserOutput>(output));
		}

		[HttpGet("list")]
		[ProducesResponseType(typeof(ListUsersOutput), StatusCodes.Status200OK)]
		[AllowAnonymous]
		public async Task<IActionResult> List(
		CancellationToken cancellationToken,
			[FromQuery] int? Page = null,
			[FromQuery] int? PerPage = null,
			[FromQuery] string? Search = null,
			[FromQuery] string? OrderBy = null,
			[FromQuery] SearchOrder? Order = null
		)
		{
			var input = new ListUsersInput();
			if (Page is not null) input.Page = Page.Value;
			if (PerPage is not null) input.PerPage = PerPage.Value;
			if (!String.IsNullOrWhiteSpace(Search)) input.Search = Search;
			if (!String.IsNullOrWhiteSpace(OrderBy)) input.OrderBy = OrderBy;
			if (Order is not null) input.Order = Order.Value;

			var output = await _mediator.Send(input, cancellationToken);
			return Ok(new ResponseList<UserOutput>(output));
		}
	}
}
