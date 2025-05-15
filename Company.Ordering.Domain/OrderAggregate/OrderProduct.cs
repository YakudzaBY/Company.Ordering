namespace Company.Ordering.Domain.OrderAggregate;

public class OrderProduct
{
    protected OrderProduct()
    {

    }

    public OrderProduct(int productId, string? productName, int productAmount, decimal productPrice) : this()
    {
        ProductId = productId;
        ProductName = productName;
        ProductAmount = productAmount;
        ProductPrice = productPrice;
    }

    public int ProductId { get; private set; }

    public string? ProductName { get; private set; }

    public int ProductAmount { get; private set; }

    public decimal ProductPrice { get; private set; }
}
