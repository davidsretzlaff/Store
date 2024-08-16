using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Api.ApiModels.Response;
using Store.Application.UseCases.User.ActivateUser;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateUser;
using Store.Application.UseCases.User.DeactiveUser;

namespace Store.Api.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IMediator _mediator;

		public UsersController(IMediator mediator) => _mediator = mediator;

		[ProducesResponseType(typeof(ApiResponse<UserOutput>), StatusCodes.Status201Created)]
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
				new ApiResponse<UserOutput>(output)
			);
		}

		[HttpPut("{id:guid}/Activate")]
		[ProducesResponseType(typeof(ApiResponse<UserOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Activate(
		  Guid id,
		  CancellationToken cancellationToken
		)
		{
			var output = await _mediator.Send(new ActivateUserInput(id), cancellationToken);
			return Ok(new ApiResponse<UserOutput>(output));
		}

		[HttpPut("{id:guid}/Deactivate")]
		[ProducesResponseType(typeof(ApiResponse<UserOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Deactivate(
		  Guid id,
		  CancellationToken cancellationToken
		)
		{
			var output = await _mediator.Send(new DeactivateUserInput(id), cancellationToken);
			return Ok(new ApiResponse<UserOutput>(output));
		}
	}
}
