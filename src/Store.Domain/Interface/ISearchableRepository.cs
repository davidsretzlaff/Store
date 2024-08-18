using Store.Domain.SeedWork;
using Store.Domain.SeedWork.Searchable;

namespace Store.Domain.Repository
{
	public interface ISearchableRepository<Taggregate>
		where Taggregate : AggregateRoot
	{
		Task<SearchOutput<Taggregate>> Search(
			SearchInput input,
			CancellationToken cancellationToken
		);
	}
}
