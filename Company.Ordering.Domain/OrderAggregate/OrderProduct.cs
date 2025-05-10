namespace Company.Ordering.Domain.OrderAggregate;

public class OrderProduct
{
    public int OrderNumber { get; set; }

    public int ProductId { get; set; }

    public string Name { get; set; } = default!;

    public int Amount { get; set; }

    public decimal Price { get; set; }
}
