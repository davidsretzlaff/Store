using Bogus;
using Microsoft.Extensions.Configuration;
using Store.Infra.Data.EF;
using Store.Tests.Shared;

namespace Store.EndToEndTest.Base
{
	public class BaseFixture
	{
		public GenerateDataBase DataGenerator { get; }
		public HttpClient HttpClient { get; set; }
		public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
		public ApiClient ApiClient { get; set; }
		public BaseFixture() {
			DataGenerator = new GenerateDataBase();

			WebAppFactory = new CustomWebApplicationFactory<Program>();
			HttpClient = WebAppFactory.CreateClient();
			ApiClient = new ApiClient(HttpClient);
			var configuration = WebAppFactory.Services
		   .GetService(typeof(IConfiguration));
			ArgumentNullException.ThrowIfNull(configuration);
		}

		public StoreDbContext CreateDbContext(bool preserveData = false)
		 => DataGenerator.CreateDbContext("end2end-tests-db", preserveData);
	}
}
