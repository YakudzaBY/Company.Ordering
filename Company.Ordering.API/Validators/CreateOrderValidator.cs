using Company.Ordering.API.Commands;
using Company.Ordering.Domain.ProductAggregate;
using FluentValidation;

namespace Company.Ordering.API.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrder>
{
    public CreateOrderValidator(IProductsRepository productRepository)
    {
        RuleFor(order => order.InvoiceEmailAddress)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("The email address is invalid");

        RuleForEach(order => order.Products)
            .MustAsync(async (product, cancellation) =>
            {
                var isInStock = await productRepository.IsInStock(product.ProductId, product.ProductAmount);
                return isInStock;
            })
            .WithMessage("The product is out of stock");
    }
}
