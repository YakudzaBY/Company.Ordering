using Company.Ordering.Domain.ProductAggregate;
using FluentValidation;

namespace Company.Ordering.Domain.OrderAggregate;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator(IProductsRepository productRepository)
    {
        RuleFor(order => order.InvoiceEmailAddress)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("The email address is invalid");

        RuleForEach(order => order.Products)
            .MustAsync(async (product, cancellation) =>
            {
                var isInStock = await productRepository.IsInStock(product.ProductId, product.Amount);
                return isInStock;
            })
            .WithMessage("The product is out of stock");
    }
}
