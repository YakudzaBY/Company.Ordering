using Company.Ordering.API.Commands;
using Company.Ordering.Domain.OrderAggregate;
using FluentValidation;

namespace Company.Ordering.API.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrder>
{
    public CreateOrderValidator(IValidator<Order> orderValidator)
    {
        Include(orderValidator);
    }
}
