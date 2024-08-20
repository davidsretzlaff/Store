using Store.Domain.Enum;

namespace Store.Domain.Extensions
{
    public static class UserStatusExtensions
    {
        public static UserStatus ToUserStatus(this string statusString)
        => statusString switch
        {
            "Active" => UserStatus.Active,
            "Inactive" => UserStatus.Inactive,
            "Waiting" => UserStatus.Waiting,
            _ => throw new ArgumentOutOfRangeException(nameof(statusString))
        };

        public static string ToUserStatusString(this UserStatus statusString)
        => statusString switch
        {
            UserStatus.Active => "Active",
            UserStatus.Inactive => "Inactive",
            UserStatus.Waiting => "Waiting",
            _ => throw new ArgumentOutOfRangeException(nameof(statusString))
		};
    }
}
