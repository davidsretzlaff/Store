﻿
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
		}

		public Validate()
		{

		}
	}
}
