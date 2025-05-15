namespace Company.Ordering.Domain.Aggregates.ProductAggregate;

public class Product : Entity, IAggregateRoot
{
    protected Product()
    {

    }

    public Product(int stock) : this()
    {
        Stock = stock;
    }

    public int Stock { get; private set; }
}
