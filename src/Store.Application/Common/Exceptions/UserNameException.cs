namespace Store.Application.Common.Exceptions
{
    public class UserNameException : ApplicationException
    {
        public UserNameException(string? message) : base(message)
        {

        }

        public static void ThrowIfUserNameExist(string exceptionMessage)
        {
            throw new UserNameException(exceptionMessage);
        }
    }
}
