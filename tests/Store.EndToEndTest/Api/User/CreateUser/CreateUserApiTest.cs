﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Common.Models.Response;
using Store.Application.UseCases.User.Common;
using System.Net;
using System.Text.Json;

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
			// Arrange
			var input = _fixture.getExampleInput();

			// Act
			var (response, output) = await _fixture.ApiClient.Post<Response<UserOutput>>("/users/create", input);

			// Assert
			response.Should().NotBeNull();
			response!.StatusCode.Should().Be(HttpStatusCode.Created);
			output.Should().NotBeNull();
			output.Data.Should().NotBeNull();
			output.Data.UserName.Should().Be(input.UserName);
			output.Data.BusinessName.Should().Be(input.BusinessName);
			output.Data.CorporateName.Should().Be(input.CorporateName);
			output.Data.Id.Should().NotBeEmpty();
			output.Data.Status.Should().Be("Waiting");

			var dbCategory = await _fixture.Persistence.GetById(output.Data.Id);
			dbCategory.Should().NotBeNull();
			dbCategory!.UserName.Should().Be(input.UserName);
			dbCategory.CorporateName.Should().Be(input.CorporateName);
			dbCategory.CompanyIdentificationNumber.Value.Should().Be(input.Cnpj);
		}

		[Fact(DisplayName = nameof(Error_When_CantInstantiateAggregate))]
		[Trait("EndToEnd/API", "Category/Create - Endpoints")]
		public async Task Error_When_CantInstantiateAggregate()
		{
			// Arrange
			var invalidInput = _fixture.getInvalidInput();
			
			// Act
			var (response, output) = await _fixture.ApiClient.Post<ProblemDetails>("/users/create", invalidInput);


			// Assert
			response.Should().NotBeNull();
			response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
			output.Should().NotBeNull();
			output!.Title.Should().Be("One or more validation errors ocurred");
			output.Type.Should().Be("UnprocessableEntity");
			output.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
			output.Detail.Should().Be("UserName should be at least 4 characters long");
		}
	}
}
