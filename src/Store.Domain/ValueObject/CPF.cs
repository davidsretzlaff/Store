
using Store.Domain.Validation;

namespace Store.Domain.ValueObject
{
	public class CPF
	{
		public string Value {  get; private set; }

		public CPF(string value)
		{
			Value = value;
			DomainValidation.Throw(IsValid(), "CPF is invalid");
		}


		public bool IsValid() 
		{
			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			
			string tempCpf;
			string digito;
			int soma;
			int resto;
			
			Value = Value.Trim();
			Value = Value.Replace(".", "").Replace("-", "");
			
			if (Value.Length != 11)
				return false;
			
			tempCpf = Value.Substring(0, 9);
			soma = 0;

			for (int i = 0; i < 9; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
			
			resto = soma % 11;
			
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			
			digito = resto.ToString();
			tempCpf = tempCpf + digito;
			soma = 0;
			
			for (int i = 0; i < 10; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
			
			resto = soma % 11;
			
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			
			digito = digito + resto.ToString();
			return Value.EndsWith(digito);
		}
	}
}
