using Store.Application.Common.ApiResponse.PaginatedList;
using Store.Application.UseCases.User.Common;

namespace Store.Application.UseCases.User.ListUsers
{
	public class ListUsersOutput : PaginatedListOutput<UserOutput>
	{
		public ListUsersOutput(int page, int perPage, int total, IReadOnlyList<UserOutput> items) 
			: base(page, perPage, total, items)
		{
		}
	}
}
