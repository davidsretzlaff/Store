using FluentAssertions;
using Store.Domain.Enum;
using Store.Domain.Extensions;

namespace Store.UnitTest.Extensions
{
	public class UserStatusExtentionsTest
	{
		[Theory(DisplayName = nameof(StringToUserStatus))]
		[Trait("Domain", "UserStatus - Extensions")]
		[InlineData("Active", UserStatus.Active)]
		[InlineData("Inactive", UserStatus.Inactive)]
		[InlineData("Waiting", UserStatus.Waiting)]
		public void StringToUserStatus(string enumString, UserStatus expectedStatus)
		{
			enumString.ToUserStatus().Should().Be(expectedStatus);
		}

		[Fact(DisplayName = nameof(ThrowsExceptionWhenInvalidString))]
		[Trait("Domain", "UserStatus - Extensions")]
		public void ThrowsExceptionWhenInvalidString()
		{
			var action = () => "Invalid".ToUserStatus();
			action.Should().Throw<ArgumentOutOfRangeException>();
		}

		[Theory(DisplayName = nameof(StringToUserStatus))]
		[Trait("Domain", "UserStatus - Extensions")]
		[InlineData(UserStatus.Active, "Active")]
		[InlineData(UserStatus.Inactive, "Inactive")]
		[InlineData(UserStatus.Waiting, "Waiting")]
		public void UserStatusToString(UserStatus expectedStatus, string enumString)
		{
			enumString.ToUserStatus().Should().Be(expectedStatus);
		}
	}
}
