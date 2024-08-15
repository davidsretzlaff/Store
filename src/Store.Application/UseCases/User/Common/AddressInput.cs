namespace Store.Application.UseCases.User.Common
{
	public record AddressInput
	(
		string Street,
		string City,
		string State,
		string Country,
		string ZipCode
	);
}
