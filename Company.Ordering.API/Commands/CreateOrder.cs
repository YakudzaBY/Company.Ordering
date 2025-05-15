using Company.Ordering.API.Models;
using MediatR;

namespace Company.Ordering.API.Commands
{
    public class CreateOrder(string invoiceEmailAddress,
        IEnumerable<OrderProduct> products,
        DateTime createdAt,
        string invoiceAddress = default,
        string invoiceCreditCardNumber = default) : IRequest<int>
    {
        public virtual IEnumerable<OrderProduct> Products => products;

        public string? InvoiceAddress => invoiceAddress;

        public string InvoiceEmailAddress => InvoiceEmailAddress;

        public string? InvoiceCreditCardNumber => InvoiceCreditCardNumber;

        public DateTime CreatedAt => createdAt;

    }
}
