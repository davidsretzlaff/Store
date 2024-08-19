namespace Store.Api.Models.CreateProduct
{
	public record ApiCreateProductInput(
		int Id,
		string Title,
		decimal Price,
		string Description,
		string Category
	);
}
