using FluentValidation;

namespace Company.Ordering.Domain.OrderAggregate;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(order => order.InvoiceEmailAddress)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid Email Address");
    }
}
