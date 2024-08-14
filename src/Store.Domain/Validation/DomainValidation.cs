using Store.Domain.Exceptions;

namespace Store.Domain.Validation
{
	public class DomainValidation
	{
		public static void NotNull(object? target, string fieldName)
		{
			if (target is null)
				throw new EntityValidationException($"{fieldName} should not be null");
		}

		public static void MaxLength(int[] target, int maxLength, string fieldName)
		{
			for (int i = 0; i < target.Length; i++)
			{
				int encoded = target[i];
				string encodedStr = encoded.ToString();
				if (encodedStr.Length > maxLength)
				{
					throw new EntityValidationException($"{fieldName} should be less or equal {maxLength} characters long");
				}
			}
		}
	}
}
