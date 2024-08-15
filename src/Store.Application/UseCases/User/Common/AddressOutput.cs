namespace Store.Application.UseCases.User.Common
{
	public record AddressOutput
	(
		string Street,
		string City,
		string State,
		string Country,
		string ZipCode
	);
}
