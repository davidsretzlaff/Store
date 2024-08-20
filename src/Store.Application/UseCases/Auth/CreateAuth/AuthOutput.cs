
namespace Store.Application.UseCases.Auth.CreateAuth
{
	public record AuthOutput(string UserName, string Cnpj, string Token)
	{
		public static AuthOutput FromUser(Domain.Entity.User user, string token)
		{
			return new AuthOutput(
				  user.UserName,
				  user.Cnpj.Value,
				  token
			   );
		}
	}
}
