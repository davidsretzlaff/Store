﻿using FluentAssertions;
using Store.Api.ApiModels.Response;
using Store.Application.UseCases.User.Common;
using System.Net;

namespace Store.EndToEndTest.Api.User.CreateUser
{
    [Collection(nameof(CreateUserApiTestFixture))]
	public class CreateUserApiTest
	{
		private readonly CreateUserApiTestFixture _fixture;
		public CreateUserApiTest(CreateUserApiTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(CreateUser))]
		[Trait("EndToEnd/API", "User/Create - Endpoints")]
		public async Task CreateUser()
		{
			{
				// Arrange
				var input = _fixture.getExampleInput();

				// Act
				var (response, output) = await _fixture.ApiClient.Post<ApiResponse<UserOutput>>("/users", input);

				// Assert
				response.Should().NotBeNull();
				response!.StatusCode.Should().Be(HttpStatusCode.Created);
				output.Should().NotBeNull();
				output.Data.Should().NotBeNull();
				output.Data.Name.Should().Be(input.Name);
				output.Data.BusinessName.Should().Be(input.BusinessName);
				output.Data.CorporateName.Should().Be(input.CorporateName);
				output.Data.Id.Should().NotBeEmpty();
				output.Data.Status.Should().Be("Waiting");

				var dbCategory = await _fixture.Persistence.GetById(output.Data.Id);
				dbCategory.Should().NotBeNull();
				dbCategory!.Name.Should().Be(input.Name);
				dbCategory.CorporateName.Should().Be(input.CorporateName);
				dbCategory.CompanyRegistrationNumber.Should().Be(input.CompanyRegistrationNumber);
			}
		}
	}
}
