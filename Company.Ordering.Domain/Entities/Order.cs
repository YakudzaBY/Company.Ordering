namespace Company.Ordering.Domain.Entities;

public class Order
{
    public int Number { get; set; }

    public ICollection<Product> Products { get; set; }

    public string InvoiceAddress { get; set; }

    public string InvoiceEmailAddress { get; set; }

    public string InvoiceCreditCardNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}
