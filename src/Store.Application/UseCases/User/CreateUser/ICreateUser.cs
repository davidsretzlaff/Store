using MediatR;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateAuthenticate;

namespace Store.Application.UseCases.User.CreateUser
{
    public interface ICreateUser : IRequestHandler<CreateUserInput, UserOutput>
	{
	}
}
