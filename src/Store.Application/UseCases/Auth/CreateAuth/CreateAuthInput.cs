using MediatR;

namespace Store.Application.UseCases.Auth.CreateAuth
{ 
	public record CreateAuthInput (
		string UserName,
		string Password
	): IRequest<AuthOutput>;
}
