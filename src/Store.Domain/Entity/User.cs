using Store.Domain.Enum;
using Store.Domain.Validation;
using Store.Domain.ValueObject;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.Emit;

namespace Store.Domain.Entity
{
	public class User : SeedWork.Entity
	{
		public string BusinessName { get; private set; }
		public string CorporateName { get; private set; }
		public UserStatus Status { get; private set; }
		public string Email { get; private set; }
		public string SiteUrl { get; private set; }
		public string Phone {  get; private set; }
		public string CompanyRegistrationNumber {  get; private set; }
		public Address Address { get; private set; }

        public User(
			string businessName, 
			string corporateName,
			UserStatus status,
			string email,
			string siteUrl,
			string phone,
			string companyRegistrationNumber,
			string street, 
			string city, 
			string state, 
			string country, 
			string zipCode)
        {
            this.BusinessName = businessName;
			this.CorporateName = corporateName;
			this.Status = status; // criar validador proprio
			this.Email = email;// criar validador proprio
			this.SiteUrl = siteUrl;
			this.Phone = phone; // validar proprio
			this.CompanyRegistrationNumber = companyRegistrationNumber; // validar proprio
			this.Address = new Address(street, city, state, country, zipCode);
		}

		public void Validate()
		{
			DomainValidation.NotNullOrEmpty(BusinessName, nameof(BusinessName));
			DomainValidation.MaxLength(BusinessName, 100, nameof(BusinessName));
			DomainValidation.MinLength(BusinessName, 4, nameof(BusinessName));

			DomainValidation.NotNullOrEmpty(CorporateName, nameof(CorporateName));
			DomainValidation.MaxLength(CorporateName, 100, nameof(CorporateName));
			DomainValidation.MinLength(CorporateName, 3, nameof(CorporateName));

			DomainValidation.NotNullOrEmpty(SiteUrl, nameof(SiteUrl));
			DomainValidation.MaxLength(SiteUrl, 100, nameof(SiteUrl));
			DomainValidation.MinLength(SiteUrl, 3, nameof(SiteUrl));

		}
	}
}
