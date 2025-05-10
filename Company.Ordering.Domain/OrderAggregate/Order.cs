using System.ComponentModel.DataAnnotations;

namespace Company.Ordering.Domain.OrderAggregate;

public class Order
{
    public int Number { get; set; }

    public ICollection<OrderProduct> Products { get; set; }

    public string InvoiceAddress { get; set; }

    public string InvoiceEmailAddress { get; set; }

    public string InvoiceCreditCardNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}
