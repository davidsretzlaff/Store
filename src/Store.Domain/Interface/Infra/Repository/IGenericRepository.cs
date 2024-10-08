﻿using Store.Domain.SeedWork;
namespace Store.Domain.Interface.Infra.Repository
{
    public interface IGenericRepository<TAggregate> : IRepository
        where TAggregate : AggregateRoot
    {
        public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
    }
}
