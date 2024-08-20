using Store.Domain.Entity;
using Store.Domain.ValueObject;

namespace Store.Application.Common.Exceptions
{
	public class InvalidOrderOwnershipException : ApplicationException
	{
		public InvalidOrderOwnershipException(string message) : base(message)
		{ }

		public static void ThrowIfNotOwnership(object? @object, string Cnpj , string exceptionMessage)
		{
			if (@object is Order order)
			{
				if (order.Cnpj.Value != CNPJ.RemoveNonDigits(Cnpj)) throw new InvalidOrderOwnershipException(exceptionMessage);
			}
			//if (@object is CANCELEDorder)
			//{
			//	if (order.Cnpj != Cnpj) throw new DuplicateException(exceptionMessage);
			//}
		}
	}
}
