﻿using MediatR;
using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.User.CreateUser
{
    public interface ICreateUser : IRequestHandler<CreateUserInput, UserOutput>
	{
	}
}
