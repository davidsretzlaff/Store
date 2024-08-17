using FluentAssertions;
using Store.Application.Common.Models.Response;
using Store.Application.UseCases.User.Common;
using Store.Domain.Extensions;
using System.Net;

namespace Store.EndToEndTest.Api.User.DeactivateUser
{

	[Collection(nameof(DeactivateUserApiTestFixture))]
	public class DeactivateUserApiTest
	{
		private readonly DeactivateUserApiTestFixture _fixture;
		public DeactivateUserApiTest(DeactivateUserApiTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(ActivateUser))]
		[Trait("EndToEnd/API", "User/Create - Endpoints")]
		public async Task DeactivateUser()
		{
			{
				// Arrange
				var exampleUserList = _fixture.GetExampleUserList(5);
				await _fixture.Persistence.InsertList(exampleUserList);
				var exampleUser = exampleUserList[2];
				exampleUser.Deactivate();

				// Act
				var (response, output) = await _fixture.ApiClient.
					Put<Response<UserOutput>>($"/users/{exampleUser.Id}/deactivate");

				// Assert
				response.Should().NotBeNull();
				response!.StatusCode.Should().Be(HttpStatusCode.OK);
				output.Should().NotBeNull();
				output!.Data.Should().NotBeNull();
				output.Data.Id.Should().Be(exampleUser.Id);
				output.Data.Status.Should().Be(exampleUser.Status.ToString());
				output.Data.Status.Should().Be(Domain.Enum.UserStatus.Inactive.ToStringStatus());

				var dbUser = await _fixture.Persistence.GetById(output.Data.Id);
				dbUser.Should().NotBeNull();
				dbUser!.UserName.Should().Be(exampleUser.UserName);
				dbUser.Status.Should().Be(exampleUser.Status);
				dbUser.Status.Should().Be(Domain.Enum.UserStatus.Inactive);
			}
		}
	}
}
