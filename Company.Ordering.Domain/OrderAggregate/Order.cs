using System.ComponentModel.DataAnnotations;

namespace Company.Ordering.Domain.OrderAggregate;

public class Order : IAggregateRoot
{
    public int Number { get; set; }

    public virtual Guid Guid { get; set; }

    public ICollection<OrderProduct> Products { get; set; } = default!;

    public string? InvoiceAddress { get; set; }

    public string InvoiceEmailAddress { get; set; } = default!;

    public string? InvoiceCreditCardNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}
