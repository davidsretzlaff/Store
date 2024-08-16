using MediatR;
using Store.Application.UseCases.Authenticate.CreateAuthenticate;
using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.User.CreateAuthenticate
{
    public record CreateAuthInput (
		string UserName,
		string Password
	): IRequest<AuthOutput>;
}
