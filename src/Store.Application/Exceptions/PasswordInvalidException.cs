
namespace Store.Application.Exceptions
{
	public class PasswordInvalidException : ApplicationException
	{
		public PasswordInvalidException(string? message) : base(message)
		{

		}

		public static void ThrowIfPasswordInvalid()
		{
			throw new PasswordInvalidException("Password Invalid");
		}
	}
}
