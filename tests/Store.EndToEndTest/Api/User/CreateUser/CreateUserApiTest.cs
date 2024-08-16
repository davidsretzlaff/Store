using Store.Application.UseCases.User.CreateUser.Common;
using System.Net;

namespace Store.EndToEndTest.Api.User.CreateUser
{
	[Collection(nameof(CreateUserApiTestFixture))]
	public class CreateUserApiTest
	{
		private readonly CreateUserApiTestFixture _fixture;
		public CreateUserApiTest(CreateUserApiTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(CreateCategory))]
		[Trait("EndToEnd/API", "User/Create - Endpoints")]
		public async Task CreateCategory()
		{
			{
				var input = _fixture.getExampleInput();

				var (response, output) = await _fixture.ApiClient.Post<ApiResponse<UserOutput>>("/users", input);

				response.Should().NotBeNull();
				response!.StatusCode.Should().Be(HttpStatusCode.Created);
				output.Should().NotBeNull();
				output.Data.Should().NotBeNull();
				output.Data.Name.Should().Be(input.Name);
				output.Data.Description.Should().Be(input.Description);
				output.Data.IsActive.Should().Be(input.IsActive);
				output.Data.Id.Should().NotBeEmpty();
				output.Data.CreatedAt.Should().NotBeSameDateAs(default);

				var dbCategory = await _fixture.Persistence.GetById(output.Data.Id);
				dbCategory.Should().NotBeNull();
				dbCategory!.Name.Should().Be(input.Name);
				dbCategory.Description.Should().Be(input.Description);
			}
		}

	}
}
