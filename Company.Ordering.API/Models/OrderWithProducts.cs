namespace Company.Ordering.API.Models
{
    public class OrderWithProducts
    {
        public int Number { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; } = default!;

        public string? InvoiceAddress { get; set; }

        public string InvoiceEmailAddress { get; set; } = default!;

        public string? InvoiceCreditCardNumber { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
