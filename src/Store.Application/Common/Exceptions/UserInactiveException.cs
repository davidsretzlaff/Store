namespace Store.Application.Common.Exceptions
{
	public class UserInactiveException : ApplicationException
	{
		public UserInactiveException(string message) : base(message)
		{ }

		public static void Throw(string exceptionMessage)
		{
			throw new UserInactiveException(exceptionMessage);
		}
	}
}
