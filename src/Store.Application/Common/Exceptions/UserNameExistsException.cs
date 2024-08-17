namespace Store.Application.Common.Exceptions
{
    public class UserNameExistsException : ApplicationException
    {
        public UserNameExistsException(string message) : base(message)
        {}

		public static void ThrowIfNull(string exceptionMessage)
		{
			new UserNameExistsException(exceptionMessage);
		}
	}
}
