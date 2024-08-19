using Store.Domain.SeedWork;
using Store.Domain.SeedWork.Searchable;

namespace Store.Domain.Interface.Infra.Repository
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
