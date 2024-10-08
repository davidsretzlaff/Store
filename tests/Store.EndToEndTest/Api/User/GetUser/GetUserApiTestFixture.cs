﻿using Store.Application.UseCases.User.ActivateUser;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.CreateUser;
using Store.EndToEndTest.Api.User.Common;
using Store.Tests.Shared;

namespace Store.EndToEndTest.Api.User.GetUser
{
    [CollectionDefinition(nameof(GetUserApiTestFixture))]
    public class CreateCategoryApiTestFixtureCollection
        : ICollectionFixture<GetUserApiTestFixture>
    { }
    public class GetUserApiTestFixture : UserBaseFixture
    {
        public UserDataGenerator DataGenerator { get; }

        public GetUserApiTestFixture() => DataGenerator = new UserDataGenerator();
        public List<Domain.Entity.User> GetExampleUserList(int quantity = 15) => DataGenerator.GetExampleUserList(quantity);
	}
}
