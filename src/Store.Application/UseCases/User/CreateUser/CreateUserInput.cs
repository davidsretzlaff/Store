﻿using MediatR;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateUser.Common;

namespace Store.Application.UseCases.User.CreateUser
{
	public record CreateUserInput (
		string BusinessName,
		string CorporateName,
		string Email,
		string SiteUrl,
		string Phone,
		string CompanyRegistrationNumber,
		AddressInput Address
	): IRequest<UserOutput>;
}
