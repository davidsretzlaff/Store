using Store.Domain.Enum;
using Store.Domain.Exceptions;
using Store.Domain.Validation;
using Store.Domain.ValueObject;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

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
			this.Status = UserStatus.Waiting;
			this.Email = email;
			this.SiteUrl = siteUrl;
			this.Phone = phone;
			this.CompanyRegistrationNumber = companyRegistrationNumber;
			this.Address = new Address(street, city, state, country, zipCode);
			this.Validate();
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
			DomainValidation.NotNullOrEmpty(CompanyRegistrationNumber, nameof(CompanyRegistrationNumber));

			DomainValidation.ValidateEmail(Email, nameof(Email));
			DomainValidation.ValidatePhone(Phone, nameof(Phone));
		}

		public void Activate()
		{
			Status = UserStatus.Active;
		}

		public void Deactivate()
		{
			Status = UserStatus.Inactive;
		}
	}
}
