

using Store.Application.UseCases.User.Common;
using Store.Domain.Repository;

namespace Store.Application.UseCases.User.ListUsers
{
	public class ListUsers : IListUsers
	{
		private readonly IUserRepository _repository;

		public ListUsers(IUserRepository repository) => _repository = repository;

		public async Task<ListUsersOutput> Handle(ListUsersInput request, CancellationToken cancellationToken)
		{
			var searchOutput = await _repository.Search(
				new Domain.SeedWork.Searchable.SearchInput(
						request.Page,
						request.PerPage,
						request.Search,
						request.Search,
						request.Dir
					), 
					cancellationToken
				);

			return new ListUsersOutput(
				searchOutput.CurrentPage,
				searchOutput.PerPage,
				searchOutput.Total, 
				searchOutput.Items.Select(UserOutput.FromUser).ToList()
				);
		}
	}
}
