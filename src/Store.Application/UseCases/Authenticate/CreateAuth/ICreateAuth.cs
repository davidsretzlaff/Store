using MediatR;
using Store.Application.UseCases.Authenticate.CreateAuthenticate;

namespace Store.Application.UseCases.User.CreateAuthenticate
{
    public interface ICreateAuth : IRequestHandler<CreateAuthInput, AuthOutput>
	{
	}
}
