using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Api.ApiModels.Response;
using Store.Application.UseCases.User.CreateAuthenticate;
using Repository = Store.Infra.Data.EF.Repositories;
using System.Net;
using Store.Application.UseCases.Authenticate.CreateAuthenticate;

namespace Store.EndToEndTest.Api.Auth.CreateAuth
{
    [Collection(nameof(CreateAuthApiTestFixture))]
    public class CreateAuthApiTest
    {
        private readonly CreateAuthApiTestFixture _fixture;
        public CreateAuthApiTest(CreateAuthApiTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(CreateAuth))]
        [Trait("EndToEnd/API", "Auth/Create - Endpoints")]
        public async Task CreateAuth()
        {
            {
                // Arrange
				var user = _fixture.DataGenerator.GetValidUser();
				var context = _fixture.CreateDbContext();
				var repository = new Repository.UserRepository(context);
				await repository.Insert(user, CancellationToken.None);
				context.SaveChanges();
                var input = _fixture.DataGenerator.GetCreateAuthInput(user);

				// Act
				var (response, output) = await _fixture.ApiClient.Post<ApiResponse<AuthOutput>>("/Auth", input);

                // Assert
                response.Should().NotBeNull();
                response!.StatusCode.Should().Be(HttpStatusCode.Created);
                output.Should().NotBeNull();
                output.Data.Should().NotBeNull();
                output.Data.UserName.Should().Be(input.UserName);
                output.Data.Token.Should().BeNullOrEmpty();
            }
        }

        [Fact(DisplayName = nameof(Error_When_CantInstantiateAggregate))]
        [Trait("EndToEnd/API", "Category/Create - Endpoints")]
        public async Task Error_When_CantInstantiateAggregate()
        {
            // Arrange
            var invalidInput = _fixture.DataGenerator.GetCreateUserInput();

            // Act
            var (response, output) = await _fixture.ApiClient.Post<ProblemDetails>("/users", invalidInput);


            // Assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            output.Should().NotBeNull();
            output!.Title.Should().Be("One or more validation errors ocurred");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be("Name should be at least 4 characters long");
        }
    }
}
