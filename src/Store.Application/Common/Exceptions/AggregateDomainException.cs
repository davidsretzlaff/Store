namespace Store.Application.Common.Exceptions
{
	public class AggregateDomainException : ApplicationException
	{
		public AggregateDomainException(string? message) : base(message)
		{
		}

		public static void ThrowIfNull(object? @object, string exceptionMessage)
		{
			if (@object == null) throw new AggregateDomainException(exceptionMessage);

			if (@object is List<int> list)
			{
				if (list.Count == 0)
				{
					throw new AggregateDomainException(exceptionMessage);
				}
			}
		}
	}
}
