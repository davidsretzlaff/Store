using Store.Domain.Entity;
using DomainValueObject = Store.Domain.ValueObject;

namespace Store.Application.Common.Exceptions
{
	public class InvalidOrderOwnershipException : ApplicationException
	{
		public InvalidOrderOwnershipException(string message) : base(message)
		{ }

		public static void ThrowIfNotOwnership(object? @object, string? cnpj , string exceptionMessage)
		{
			if (@object is Order order)
			{
				if (order.CompanyIdentificationNumber.Value != DomainValueObject.Cnpj.RemoveNonDigits(cnpj)) throw new InvalidOrderOwnershipException(exceptionMessage);
			}
		}
	}
}
