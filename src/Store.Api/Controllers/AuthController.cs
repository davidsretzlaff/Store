using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Api.ApiModels.Response;
using Store.Application.UseCases.Auth.CreateAuth;

namespace Store.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[AllowAnonymous]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;
		public AuthController(IMediator mediator) => _mediator = mediator;

		[ProducesResponseType(typeof(ApiResponse<AuthOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Authenticate(
		  [FromBody] CreateAuthInput input,
		  CancellationToken cancellationToken
		)
		{
			var output = await _mediator.Send(input, cancellationToken);
			return CreatedAtAction(
				nameof(Authenticate),
				new { output.UserName },
				new ApiResponse<AuthOutput>(output)
			);
		}
	}
}
