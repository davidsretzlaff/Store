using MediatR;
using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.User.GetUser
{
	public record GetUserInput(Guid Id) : IRequest<UserOutput>;
}
