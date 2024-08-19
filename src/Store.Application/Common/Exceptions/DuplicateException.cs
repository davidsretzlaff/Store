namespace Store.Application.Common.Exceptions
{
	public class DuplicateException : ApplicationException
	{
		public DuplicateException(string message) : base(message)
		{ }

		public static void ThrowIfHasValue(object @object, string exceptionMessage)
		{
			if (@object != null) 
			{
				throw new DuplicateException(exceptionMessage);
			}
		}
	}
}
