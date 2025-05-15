namespace Company.Ordering.Domain.OrderAggregate;

public class OrderProduct
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int ProductAmount { get; set; }

    public decimal ProductPrice { get; set; }
}
