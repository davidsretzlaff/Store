namespace Store.Domain.Interface.Application
{
	public interface IUserValidation
	{
		Task IsUserActive(string CompanyRegisterNumber, CancellationToken cancellationToken);
	}
}
