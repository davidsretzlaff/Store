namespace Store.Application.Common.Exceptions
{
    public class CompanyRegistrationNumberExistsException : ApplicationException
    {
        public CompanyRegistrationNumberExistsException(string message) : base(message)
        {}

		public static void ThrowIfNull(string exceptionMessage)
		{
			throw new CompanyRegistrationNumberExistsException(exceptionMessage);
		}
	}
}
