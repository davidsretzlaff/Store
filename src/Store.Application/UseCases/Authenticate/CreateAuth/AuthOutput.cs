
namespace Store.Application.UseCases.Authenticate.CreateAuthenticate
{
	public record AuthOutput(string UserName, string Token)
	{
		public static AuthOutput FromUser(Domain.Entity.User user, string token)
		{
			return new AuthOutput(
				  user.UserName,
				  token
			   );
		}
	}
}
