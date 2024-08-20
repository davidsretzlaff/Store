namespace Store.Domain.Interface.Application
{
	public interface IUserValidation
	{
		Task IsUserActive(string? companyIdentificationNumber, CancellationToken cancellationToken);
	}
}
