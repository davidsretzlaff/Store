namespace Store.Application.Common.Exceptions
{
    public class RelatedAggregateException : ApplicationException
    {
        public RelatedAggregateException(string? message) : base(message)
        {
        }

        public static void ThrowIfNull(object? @object, string exceptionMessage)
        {
            if (@object == null) throw new RelatedAggregateException(exceptionMessage);
			
			if (@object is List<int> list)
			{
				if (list.Count == 0)
				{
					throw new RelatedAggregateException(exceptionMessage);
				}
			}
		}
		
		public static void Throw(string exceptionMessage)
		{
			throw new RelatedAggregateException(exceptionMessage);
		}
	}
}
