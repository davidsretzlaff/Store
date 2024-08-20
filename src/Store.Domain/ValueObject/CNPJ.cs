
using Store.Domain.Validation;
using System.Text.RegularExpressions;
namespace Store.Domain.ValueObject
{
	public class CNPJ
	{
		public string Value { get; private set; }

		public CNPJ(string cnpj) 
		{
			Value = cnpj;
			DomainValidation.Throw(isValid(), "CNPJ is invalid");
		}
		public CNPJ() { }
		public bool isValid()
		{
			int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int soma;
			int resto;
			string digito;
			string tempAddress;

			Value = Value.Trim();
			Value = Value.Replace(".", "").Replace("-", "").Replace("/", "");

			if (Value.Length != 14)
				return false;

			tempAddress = Value.Substring(0, 12);

			soma = 0;
			for (int i = 0; i < 12; i++)
				soma += int.Parse(tempAddress[i].ToString()) * multiplicador1[i];

			resto = (soma % 11);
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = resto.ToString();
			tempAddress = tempAddress + digito;
			soma = 0;

			for (int i = 0; i < 13; i++)
				soma += int.Parse(tempAddress[i].ToString()) * multiplicador2[i];

			resto = (soma % 11);
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = digito + resto.ToString();
			return Value.EndsWith(digito);
		}

		public static string RemoveNonDigits(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			return Regex.Replace(input, @"\D", "");
		}

		public bool isCNPJMatching(string newCnpj)
		{
			return Value.Equals(RemoveNonDigits(newCnpj), StringComparison.Ordinal);
			
		}
	}
}
