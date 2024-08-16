using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Api.ApiModels.Response;
using Store.Application.UseCases.User.Common;
using Store.Domain.Extensions;
using Store.EndToEndTest.Api.User.CreateUser;
using System.Net;

namespace Store.EndToEndTest.Api.User.GetUser
{

	[Collection(nameof(DeactivateUserApiTestFixture))]
	public class DeactivateUserApiTest
	{
		private readonly DeactivateUserApiTestFixture _fixture;
		public DeactivateUserApiTest(DeactivateUserApiTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(ActivateUser))]
		[Trait("EndToEnd/API", "User/Get - Endpoints")]
		public async Task GetUser()
		{
			{
				// Arrange
				var exampleUserList = _fixture.GetExampleUserList(5);
				await _fixture.Persistence.InsertList(exampleUserList);
				var exampleUser = exampleUserList[2];

				// Act
				var (response, output) = await _fixture.ApiClient.
					Get<ApiResponse<UserOutput>>($"/users/{exampleUser.Id}");

				// Assert
				response.Should().NotBeNull();
				response!.StatusCode.Should().Be(HttpStatusCode.OK);
				output.Should().NotBeNull();
				output!.Data.Should().NotBeNull();
				output.Data.Id.Should().Be(exampleUser.Id);
				output.Data.Status.Should().Be(exampleUser.Status.ToString());

				var dbUser = await _fixture.Persistence.GetById(output.Data.Id);
				dbUser.Should().NotBeNull();
				dbUser!.Name.Should().Be(exampleUser.Name);
				dbUser.Status.Should().Be(exampleUser.Status);
				dbUser.Status.Should().Be(Domain.Enum.UserStatus.Waiting);
			}
		}

		[Fact(DisplayName = nameof(Error_When_NotFound))]
		[Trait("EndToEnd/API", "User/Get - Endpoints")]
		public async Task Error_When_NotFound()
		{
			{
				// Arrange
				var exampleUserList = _fixture.GetExampleUserList(5);
				await _fixture.Persistence.InsertList(exampleUserList);
				var randomGuid = Guid.NewGuid();

				// Act
				var (response, output) = await _fixture.ApiClient.
					Get<ProblemDetails>($"/users/{randomGuid}");

				// Assert
				response.Should().NotBeNull();
				response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
				output.Should().NotBeNull();
				output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
				output.Type.Should().Be("NotFound");
				output.Title.Should().Be("Not Found");
				output.Detail.Should().Be($"User '{randomGuid}' not found.");
			}
		}
	}
}
