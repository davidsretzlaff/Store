namespace Store.Application.Common.Exceptions
{
    public class AggregateExistsException : ApplicationException
    {
        public AggregateExistsException(string? message) : base(message)
        {

        }

        public static void ThrowIfExist(string exceptionMessage)
        {
            throw new AggregateExistsException(exceptionMessage);
        }
    }
}
