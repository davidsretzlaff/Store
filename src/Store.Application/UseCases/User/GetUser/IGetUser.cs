using MediatR;
using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.User.GetUser
{
	public interface IGetUser : IRequestHandler<GetUserInput, UserOutput>
	{
	}
}
