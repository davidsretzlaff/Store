using MediatR;
using Store.Application.UseCases.Authenticate.CreateAuthenticate;
using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.User.CreateUser
{
    public record CreateUserInput (
		string UserName,
		string Password,
		string BusinessName,
		string CorporateName,
		string Email,
		string SiteUrl,
		string Phone,
		string CompanyRegistrationNumber,
		AddressInput Address
	): IRequest<UserOutput>;
}
