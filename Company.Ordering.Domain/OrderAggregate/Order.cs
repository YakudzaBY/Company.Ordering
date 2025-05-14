using System.ComponentModel.DataAnnotations;

namespace Company.Ordering.Domain.OrderAggregate;

public class Order : IAggregateRoot
{
    public int OrderNumber { get; set; }

    public virtual ICollection<OrderProduct>? Products { get; set; }

    public string? InvoiceAddress { get; set; }

    public string InvoiceEmailAddress { get; set; } = default!;

    public string? InvoiceCreditCardNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}
