using MediatR;
using Store.Application.UseCases.User.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.UseCases.User.DeactiveUser
{
	public record class DeactivateUserInput(Guid id) : IRequest<UserOutput>
	{
	}
}
