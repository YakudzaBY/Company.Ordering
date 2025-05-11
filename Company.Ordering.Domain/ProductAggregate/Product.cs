namespace Company.Ordering.Domain.ProductAggregate;

public class Product : IAggregateRoot
{
    public int Id { get; set; }

    public int Stock { get; set; }
}
