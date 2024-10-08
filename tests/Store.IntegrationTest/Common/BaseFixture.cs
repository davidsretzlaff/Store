﻿using Store.Infra.Data.EF;
using Store.Tests.Shared;

namespace Store.IntegrationTest
{
	public class BaseFixture
	{
		public GenerateDataBase DataGenerator { get; }
		public BaseFixture() => DataGenerator = new GenerateDataBase();
		public StoreDbContext CreateDbContext(bool preserveData = false)
		 => DataGenerator.CreateDbContext("integration-tests-db", preserveData);
	}
}
