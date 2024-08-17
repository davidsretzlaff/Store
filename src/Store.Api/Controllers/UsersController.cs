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

		[ProducesResponseType(typeof(Response<UserOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
		public async Task<IActionResult> GetById(
		  [FromRoute] Guid id,
		  CancellationToken cancellationToken
		)
		{
			var output = await _mediator.Send(new GetUserInput(id), cancellationToken);
			return Ok(new Response<UserOutput>(output));
		}

		[HttpGet]
		[ProducesResponseType(typeof(ListUsersOutput), StatusCodes.Status200OK)]
		public async Task<IActionResult> List(
		CancellationToken cancellationToken,
			[FromQuery] int? page = null,
			[FromQuery(Name = "per_page")] int? perPage = null,
			[FromQuery] string? search = null,
			[FromQuery] string? sort = null,
			[FromQuery] SearchOrder? dir = null
		)
		{
			var input = new ListUsersInput();
			if (page is not null) input.Page = page.Value;
			if (perPage is not null) input.PerPage = perPage.Value;
			if (!String.IsNullOrWhiteSpace(search)) input.Search = search;
			if (!String.IsNullOrWhiteSpace(sort)) input.Sort = sort;
			if (dir is not null) input.Dir = dir.Value;

			var output = await _mediator.Send(input, cancellationToken);
			return Ok(new ResponseList<UserOutput>(output));
		}
	}
}
