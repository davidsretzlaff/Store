using MediatR;
using Store.Application.UseCases.Auth.CreateAuth;

namespace Store.Application.UseCases.User.Auth.CreateAuth
{
    public interface ICreateAuth : IRequestHandler<CreateAuthInput, AuthOutput>
	{
	}
}
