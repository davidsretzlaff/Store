using Store.Domain.Enum;

namespace Store.Domain.Extensions
{
	public static class CategoryExtension
	{
		public static Category ToUserStatus(this string category)
		=> category switch
		{
			"jewelery" => Category.Jewelery,
			"electronics" => Category.Electronics,
			_ => throw new ArgumentOutOfRangeException(nameof(category))
		};

		public static string ToStringStatus(this Category category)
		=> category switch
		{
			Category.Jewelery => "jewelery",
			Category.Electronics => "electronics",
		};
	}
}
