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
            .WithMessage("Invalid Email Address");

        RuleForEach(order => order.Products)
            .MustAsync(async (product, cancellation) =>
            {
                var isInStock = await productRepository.IsInStock(product.ProductId, product.Amount);
                return isInStock;
            })
            .WithMessage("Product out of stock");
    }
}
