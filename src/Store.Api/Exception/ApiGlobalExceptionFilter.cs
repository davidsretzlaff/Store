using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Store.Domain.Exceptions;
using Store.Application.Common.Exceptions;

namespace Store.Api.Exception
{
    public class ApiGlobalExceptionFilter : IExceptionFilter
	{
		private readonly IHostEnvironment _env;
		public ApiGlobalExceptionFilter(IHostEnvironment env)
			=> _env = env;

		public void OnException(ExceptionContext context)
		{
			var details = new ProblemDetails();
			var exception = context.Exception;

			if (_env.IsDevelopment())
				details.Extensions.Add("StackTrace", exception.StackTrace);

			if (exception is EntityValidationException)
			{
				details.Title = "One or more validation errors ocurred";
				details.Status = StatusCodes.Status422UnprocessableEntity;
				details.Type = "UnprocessableEntity";
				details.Detail = exception!.Message;
			}
			else if (exception is RelatedAggregateException)
			{
				details.Title = "Not Found";
				details.Status = StatusCodes.Status404NotFound;
				details.Type = "NotFound";
				details.Detail = exception!.Message;
			}
			else if (exception is UserNameExistsException)
			{
				details.Title = "Username already exists";
				details.Status = StatusCodes.Status409Conflict;
				details.Type = "NotCreated";
				details.Detail = exception!.Message;
			}
			else if (exception is CnpjExistsException)
			{
				details.Title = "CNPJ already exist";
				details.Status = StatusCodes.Status409Conflict;
				details.Type = "NotCreated";
				details.Detail = exception!.Message;
			}
			else if (exception is PasswordInvalidException) {
				details.Title = "Password invalid";
				details.Status = StatusCodes.Status401Unauthorized;
				details.Type = "NotAuth";
				details.Detail = exception!.Message;
			} 
			else if (exception is DuplicateException) {
				details.Title = "Product already exist";
				details.Status = StatusCodes.Status409Conflict;
				details.Type = "NotCreated";
				details.Detail = exception!.Message;
			}
			else if (exception is RelatedAggregateException)
			{
				details.Title = "Invalid Related Aggregate";
				details.Status = StatusCodes.Status422UnprocessableEntity;
				details.Type = "RelatedAggregate";
				details.Detail = exception!.Message;
			}
			else if (exception is UserInactiveException)
			{
				details.Title = "User is Inactive";
				details.Status = StatusCodes.Status401Unauthorized;
				details.Type = "UserInactive";
				details.Detail = exception!.Message;
			}
			else if (exception is AggregateDomainException)
			{
				details.Title = "Not Found";
				details.Status = StatusCodes.Status404NotFound;
				details.Type = "NotFound";
				details.Detail = exception!.Message;
			}
			else if (exception is InvalidOrderOwnershipException)
			{
				details.Title = "Not Owner";
				details.Status = StatusCodes.Status409Conflict;
				details.Type = "NotOwner";
				details.Detail = exception!.Message;
			}
			else
			{
				details.Title = "An unexpected error ocurred";
				details.Status = StatusCodes.Status422UnprocessableEntity;
				details.Type = "UnexpectedError";
				details.Detail = exception.Message;
			}

			context.HttpContext.Response.StatusCode = (int)details.Status;
			context.Result = new ObjectResult(details);
			context.ExceptionHandled = true;
		}
	}

}
