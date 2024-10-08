﻿using MediatR;

namespace Store.Application.UseCases.User.ListUsers
{
	public interface IListUsers : IRequestHandler<ListUsersInput, ListUsersOutput>
	{
	}
}
