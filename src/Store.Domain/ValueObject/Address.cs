
using Store.Domain.Validation;

namespace Store.Domain.ValueObject
{
	public class Address 
	{
		public String Street { get; private set; }
		public String City { get; private set; }
		public String State { get; private set; }
		public String Country { get; private set; }
		public String ZipCode { get; private set; }

		public Address(string street, string city, string state, string country, string zipcode)
		{
			Street = street;
			City = city;
			State = state;
			Country = country;
			ZipCode = zipcode;
			Validate();
		}

		public void Validate()
		{
			DomainValidation.NotNull(Street, nameof(Street));
			DomainValidation.MaxLength(Street, 100, nameof(Street));
			DomainValidation.MinLength(Street, 4, nameof(Street));

			DomainValidation.NotNull(City, nameof(City));
			DomainValidation.MaxLength(City, 30, nameof(City));
			DomainValidation.MinLength(City, 3, nameof(City));

			DomainValidation.NotNull(State, nameof(State));
			DomainValidation.MaxLength(State, 30, nameof(State));
			DomainValidation.MinLength(State, 1, nameof(State));

			DomainValidation.NotNull(Country, nameof(Country));
			DomainValidation.MaxLength(Country, 9, nameof(Country));
			DomainValidation.MinLength(Country, 7, nameof(Country));
		}
	}
}
