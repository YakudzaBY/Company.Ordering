namespace Company.Ordering.Domain.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public int Amount { get; set; };

    public decimal Price { get; set; }
}
