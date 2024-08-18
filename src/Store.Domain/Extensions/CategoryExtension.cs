using Store.Domain.Enum;

namespace Store.Domain.Extensions
{
	public static class CategoryExtension
	{
		public static Category ToCategory(this string category)
		=> category.ToLower() switch
		{
			"jewelery" => Category.Jewelery,
			"electronics" => Category.Electronics,
			_ => Category.Invalid
		};

		public static string ToCategoryString(this Category category)
		=> category switch
		{
			Category.Jewelery => "Jewelery",
			Category.Electronics => "Electronics",
		};
	}
}
