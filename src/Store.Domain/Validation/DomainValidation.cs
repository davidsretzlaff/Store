using Store.Domain.Entity;
using Store.Domain.Enum;
using Store.Domain.Exceptions;
using Store.Domain.Extensions;
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

		public static void OrderIsNotApprove(Order order)
		{
			if (!order.IsApproved())
				throw new EntityValidationException($"The current order status is '{order!.Status.ToOrderStatusString()}'. To create a delivery order, the status must be 'Approved'");
		}
		public static void DeliveryIsPending(DeliveryStatus status)
		{
			if (status != DeliveryStatus.Pending)
				throw new EntityValidationException($"Current delivery status is '{status.ToDeliveryStatusString()}'. To start the delivery process, the status must be 'Pending'");
		}

		public static void DeliveryIsInTransit(DeliveryStatus status)
		{
			if (status != DeliveryStatus.InTransit)
				throw new EntityValidationException($"Current delivery status is '{status.ToDeliveryStatusString()}'. To Proceed, the status must be 'InTransit'");
		}

		public static void NotFound(object? target, string fieldName)
		{
			if (target is null)
				throw new EntityValidationException($"{fieldName} not found");
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

		public static void MaxQuantity(int target, int maxQuantity, string fieldName)
		{
			if (target > maxQuantity)
				throw new EntityValidationException($"{fieldName} should be less or equal {maxQuantity} quantity");
		}

		public static void ValidateCategory(Category category, string fieldName)
		{
			if (category == Category.Invalid)
				throw new EntityValidationException(
					$"{fieldName} is invalid. Only {Category.Jewelery.ToCategoryString()} and " +
					$"{Category.Electronics.ToCategoryString()} are allowed.");
		}

		public static void Throw(bool isValid, string message)
		{
			if(!isValid)
				throw new EntityValidationException(message);
		}
	}
}
