namespace Store.Domain.Interface.Application
{
	public interface IUserValidation
	{
		Task IsUserActive(string Cnpj, CancellationToken cancellationToken);
	}
}
