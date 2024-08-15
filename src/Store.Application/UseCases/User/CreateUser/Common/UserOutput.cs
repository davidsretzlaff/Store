using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.User.CreateUser.Common
{
	public record UserOutput
	(
		string BusinessName,
		string CorporateName,
		string Email,
		string SiteUrl,
		string Phone,
		string CompanyRegistrationNumber,
		string Status,
		AddressInput Address
	);
}
