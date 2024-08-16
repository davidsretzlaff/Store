using Store.EndToEndTest.Base;

namespace Store.EndToEndTest.Api.User.Common
{
	public class UserBaseFixture : BaseFixture
	{
		public UserPersistence Persistence;

		public UserBaseFixture() : base()
		{
			Persistence = new UserPersistence(CreateDbContext());
		}
	}
}
