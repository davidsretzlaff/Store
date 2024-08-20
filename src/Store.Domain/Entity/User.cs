using Store.Domain.Enum;
using Store.Domain.SeedWork;
using Store.Domain.Validation;
using Store.Domain.ValueObject;

namespace Store.Domain.Entity
{
	public class User : AggregateRoot
	{
		public string UserName { get; private set; }
		public string Password { get; private set; }		
		/* Nome fantasia */
		public string BusinessName { get; private set; }
		/* Razão Social */
		public string CorporateName { get; private set; }
		public UserStatus Status { get; private set; }
		public string Email { get; private set; }
		public string SiteUrl { get; private set; }
		public string Phone {  get; private set; }
		//public string CompanyIdentificationNumber {  get; private set; }
		public Cnpj CompanyIdentificationNumber { get; set; }
		public Address Address { get; private set; }

		public User(
			string userName,
			string password,
			string businessName,
			string corporateName,
			string email,
			string siteUrl,
			string phone,
			string companyIdentificationNumber,
			Address address
		   )
		{
			UserName = userName;
			Password = password;
			BusinessName = businessName;
			CorporateName = corporateName;
			Status = UserStatus.Waiting;
			Email = email;
			SiteUrl = siteUrl;
			Phone = phone;
			CompanyIdentificationNumber = new Cnpj(companyIdentificationNumber);
			Address = address;
			Validate();
		}
		private User() 
		{
			CompanyIdentificationNumber = new Cnpj();
			UserName = string.Empty;
			Password = string.Empty;
			BusinessName = string.Empty;
			CorporateName = string.Empty;
			Status = UserStatus.Waiting;
			Email = string.Empty;
			SiteUrl = string.Empty;
			Phone = string.Empty;
			CompanyIdentificationNumber = new Cnpj();
			Address = new Address();
		}

		public void Validate()
		{
			DomainValidation.NotNullOrEmpty(UserName, nameof(UserName));
			DomainValidation.NotContainSpace(UserName, nameof(UserName));
			DomainValidation.MaxLength(UserName, 100, nameof(UserName));
			DomainValidation.MinLength(UserName, 4, nameof(UserName));

			DomainValidation.NotNullOrEmpty(BusinessName, nameof(BusinessName));
			DomainValidation.MaxLength(BusinessName, 100, nameof(BusinessName));
			DomainValidation.MinLength(BusinessName, 4, nameof(BusinessName));

			DomainValidation.NotNullOrEmpty(CorporateName, nameof(CorporateName));
			DomainValidation.MaxLength(CorporateName, 100, nameof(CorporateName));
			DomainValidation.MinLength(CorporateName, 3, nameof(CorporateName));

			DomainValidation.NotNullOrEmpty(SiteUrl, nameof(SiteUrl));

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

		public bool IsActive() 
		{ 
			return Status == UserStatus.Active;
		}

		public bool IsUserNameMatching(string newUserName)
		{
			return UserName.Equals(newUserName, StringComparison.Ordinal);
		}
	}
}
