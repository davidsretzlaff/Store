using MediatR;
using Store.Application.Common.Models.PaginatedList;
using Store.Domain.Enum;

namespace Store.Application.UseCases.User.ListUsers
{
	public class ListUsersInput : PaginatedListInput, IRequest<ListUsersOutput>
	{
		public ListUsersInput(int page, int perPage, string search, string sort, SearchOrder dir) 
			: base(page, perPage, search, sort, dir)
		{
		}

		public ListUsersInput() : base(1, 10, "", "", SearchOrder.Asc)
		{ }
	}
}
