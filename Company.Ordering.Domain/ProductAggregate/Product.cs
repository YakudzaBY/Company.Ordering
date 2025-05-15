namespace Company.Ordering.Domain.ProductAggregate;

public class Product : IAggregateRoot
{
    protected Product()
    {

    }

    public Product(int id, int stock) : this()
    {
        Id = id;
        Stock = stock;
    }

    public int Id { get; private set; }

    public int Stock { get; private set; }
}
