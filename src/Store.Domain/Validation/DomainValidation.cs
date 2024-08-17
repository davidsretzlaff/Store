using Store.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Store.Domain.Validation
{
	public class DomainValidation
	{
		public static void NotNull(object? target, string fieldName)
		{
			if (target is null)
				throw new EntityValidationException($"{fieldName} should not be null");
		}

		public static void NotNullOrEmpty(string? target, string fieldName)
		{
			if (string.IsNullOrWhiteSpace(target))
				throw new EntityValidationException($"{fieldName} should not be empty or null");
		}

		public static void NotContainSpace(string target, string fieldName)
		{
			if (target.Contains(" "))
				throw new EntityValidationException($"{fieldName} should not contain spaces");
		}

		public static void MinLength(string target, int minLength, string fieldName)
		{
			if (target.Length < minLength)
				throw new EntityValidationException($"{fieldName} should be at least {minLength} characters long");
		}

		public static void MaxLength(string target, int maxLength, string fieldName)
		{
			if (target.Length > maxLength)
				throw new EntityValidationException($"{fieldName} should be less or equal {maxLength} characters long");
		}

		public static void ValidateEmail(string email, string fieldName)
		{
			if (!IsEmailValid(email))
				throw new EntityValidationException($"{fieldName} invalid");
		}

		public static void ValidatePhone(string phone, string fieldName)
		{
			if (!IsPhoneValid(phone))
				throw new EntityValidationException($"{fieldName} invalid");
		}

		private static bool IsEmailValid(string email)
		{
			var EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
			var emailRegex = new Regex(EmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

			if (string.IsNullOrWhiteSpace(email))
				return false;

			return emailRegex.IsMatch(email);
		}

		private static bool IsPhoneValid(string phone)
		{
			var cellPhonePattern = @"^\(?[1-9]{2}\)? ?9[1-9]\d{3}-?\d{4}$";
			var cellPhoneRegex = new Regex(cellPhonePattern, RegexOptions.Compiled);

			if (string.IsNullOrWhiteSpace(phone))
				return false;

			return cellPhoneRegex.IsMatch(phone);
		}
	}
}
