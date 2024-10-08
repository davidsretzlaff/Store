﻿using Store.Domain.Validation;
using System.Text.RegularExpressions;

namespace Store.Domain.ValueObject
{
	public class Address 
	{
		public string Street { get; private set; }
		public string City { get; private set; }
		public string State { get; private set; }
		public string Country { get; private set; }
		public string ZipCode { get; private set; }

		public Address(string street, string city, string state, string country, string zipCode)
		{
			Street = street;
			City = city;
			State = state;
			Country = country;
			ZipCode = zipCode;
			Validate();
		}
		public Address()
		{
			Street = string.Empty;
			City = string.Empty;
			State = string.Empty;
			Country = string.Empty;
			ZipCode = string.Empty;
		}

		private void CleanZipCode() {
			ZipCode = Regex.Replace(ZipCode, @"\D", "");
		}
		public void Validate()
		{
			DomainValidation.NotNullOrEmpty(Street, nameof(Street));
			DomainValidation.MaxLength(Street, 100, nameof(Street));
			DomainValidation.MinLength(Street, 4, nameof(Street));

			DomainValidation.NotNullOrEmpty(City, nameof(City));
			DomainValidation.MaxLength(City, 100, nameof(City));
			DomainValidation.MinLength(City, 3, nameof(City));

			DomainValidation.NotNullOrEmpty(State, nameof(State));
			DomainValidation.MaxLength(State, 100, nameof(State));

			DomainValidation.NotNullOrEmpty(Country, nameof(Country));
			DomainValidation.MaxLength(Country, 100, nameof(Country));

			DomainValidation.NotNull(ZipCode, nameof(ZipCode));
			CleanZipCode();
			DomainValidation.MaxLength(ZipCode, 8, nameof(ZipCode));
			DomainValidation.MinLength(ZipCode, 8, nameof(ZipCode));
		}
	}
}
