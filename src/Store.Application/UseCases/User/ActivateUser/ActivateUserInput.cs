using MediatR;
using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.User.ActivateUser
{
    public record class ActivateUserInput(Guid id) : IRequest<UserOutput>
	{
	}
}
