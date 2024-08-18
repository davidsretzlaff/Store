using System.Globalization;

namespace Store.Domain.ValueObject
{
	public class Money
	{
		public decimal Amount { get; }
		public string CurrencySymbol { get; } = "U$";

		public Money(decimal amount)
		{
			Amount = amount;
		}

		public string Format()
		{
			return $"{CurrencySymbol} {Amount.ToString("N2", new CultureInfo("en-US"))}";
		}
	}
}
