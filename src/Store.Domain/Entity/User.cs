namespace Store.Domain.Entity
{
	public class User : SeedWork.Entity
	{
		public string BusinessName { get; private set; }
		public string CorporateName { get; private set; }
		public string Status { get; private set; }
		public string Email { get; private set; }
		public string SiteUrl { get; private set; }
		public string Phone {  get; private set; }
		public string CompanyRegistrationNumber {  get; private set; }

        public User(
			string businessName, 
			string corporateName, 
			string status,
			string email,
			string siteUrl,
			string phone,
			string companyRegistrationNumber)
        {
            this.BusinessName = businessName;
			this.CorporateName = corporateName;
			this.Status = status;
			this.Email = email;
			this.SiteUrl = siteUrl;
			this.Phone = phone;
			this.CompanyRegistrationNumber = companyRegistrationNumber;
        }
    }
}
