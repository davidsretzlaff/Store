using Store.Domain.Entity;

namespace Store.Application.Common.Exceptions
{
	public class InvalidOrderOwnershipException : ApplicationException
	{
		public InvalidOrderOwnershipException(string message) : base(message)
		{ }

		public static void ThrowIfNotOwnership(object? @object, string companyRegisterNumber , string exceptionMessage)
		{
			if (@object is Order order)
			{
				if (order.CompanyRegisterNumber != companyRegisterNumber) throw new InvalidOrderOwnershipException(exceptionMessage);
			}
			//if (@object is CANCELEDorder)
			//{
			//	if (order.CompanyRegisterNumber != companyRegisterNumber) throw new DuplicateException(exceptionMessage);
			//}
		}
	}
}
