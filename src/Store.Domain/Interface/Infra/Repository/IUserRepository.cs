using Store.Domain.Entity;
namespace Store.Domain.Interface.Infra.Repository
{
    public interface IUserRepository : IGenericRepository<User>, ISearchableRepository<User>
    {
        public Task<User?> GetByUserNameOrcompanyIdentificationNumber(string userName, string companyIdentificationNumber, CancellationToken cancellationToken);
        public Task<User?> GetByUserName(string userName, CancellationToken cancellationToken);
        public Task<User> Get(Guid id, CancellationToken cancellationToken);
        public Task Update(User aggregate, CancellationToken cancellationToken);
    }
}
