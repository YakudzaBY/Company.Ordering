using Company.Ordering.Domain.Events;

namespace Company.Ordering.Domain.Aggregates.OrderAggregate;

public class Order : Entity, IAggregateRoot
{

    protected Order()
    {
        _products = [];
    }

    public Order(string? invoiceAddress,
        string invoiceEmailAddress,
        string? invoiceCreditCardNumber,
        DateTime createdAt)
        : this()
    {
        InvoiceAddress = invoiceAddress;
        InvoiceEmailAddress = invoiceEmailAddress;
        InvoiceCreditCardNumber = invoiceCreditCardNumber;
        CreatedAt = createdAt;

        AddDomainEvent(new OrderCreatedDomainEvent(this));
    }

    private readonly List<OrderProduct> _products;

    public virtual IReadOnlyCollection<OrderProduct>? Products => _products;

    public string? InvoiceAddress { get; private set; }

    public string InvoiceEmailAddress { get; private set; }

    public string? InvoiceCreditCardNumber { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public async Task AddProductAsync(int productId, string? productName, int productAmount, decimal productPrice)
    {
        var product = new OrderProduct(productId, productName, productAmount, productPrice);
        _products.Add(product);
        AddDomainEvent(new OrderProductAdded(this, product));
    }
}
