

using Store.Application.UseCases.User.Common;
using Store.Domain.Interface.Infra.Repository;

namespace Store.Application.UseCases.User.ListUsers
{
    public class ListUsers : IListUsers
	{
		private readonly IUserRepository _repository;

		public ListUsers(IUserRepository repository) => _repository = repository;

		public async Task<ListUsersOutput> Handle(ListUsersInput input, CancellationToken cancellationToken)
		{
			var searchOutput = await _repository.Search(
				new Domain.SeedWork.Searchable.SearchInput(
						input.Page,
						input.PerPage,
						input.Search,
						input.OrderBy,
						input.Order,
						string.Empty
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
