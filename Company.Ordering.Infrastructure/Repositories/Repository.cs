using Company.Ordering.Domain;

namespace Company.Ordering.Infrastructure.Repositories;

public abstract class Repository<T>(IUnitOfWork uow) : IRepository<T>
    where T : IAggregateRoot
{
    public IUnitOfWork UnitOfWork => uow;
}
