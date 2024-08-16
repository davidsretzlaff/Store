using Store.Domain.Entity;

namespace Store.Application.UseCases.User.Common
{
	public record AddressInput
	(
	   string Street,
	   string City,
	   string State,
	   string Country,
	   string ZipCode
	)
	{
		public static AddressInput FromDomainAddress(Domain.ValueObject.Address address)
		{
			return new AddressInput(
				address.Street,
				address.City,
				address.State,
				address.Country,
				address.ZipCode
			);
		}

		public Domain.ValueObject.Address ToDomainAddress()
		{
			return new Domain.ValueObject.Address(
				Street,
				City,
				State,
				Country,
				ZipCode
			);
		}
	}
}
