namespace Store.Application.Common.Exceptions
{
    public class CnpjExistsException : ApplicationException
    {
        public CnpjExistsException(string message) : base(message)
        {}

		public static void ThrowIfNull(string exceptionMessage)
		{
			throw new CnpjExistsException(exceptionMessage);
		}
	}
}
