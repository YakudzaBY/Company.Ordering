namespace Company.Ordering.Domain;

public interface IUnitOfWork
{
    Task SaveEntitiesAsync(CancellationToken cancellationToken);
}
