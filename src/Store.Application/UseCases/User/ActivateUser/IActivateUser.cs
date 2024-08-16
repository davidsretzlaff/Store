using MediatR;
using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.User.ActivateUser
{
    public interface IActivateUser : IRequestHandler<ActivateUserInput, UserOutput>
	{
	}
}
