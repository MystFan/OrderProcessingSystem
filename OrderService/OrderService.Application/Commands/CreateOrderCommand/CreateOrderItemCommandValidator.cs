using FluentValidation;
using OrderService.Domain;

namespace OrderService.Application.Commands.CreateOrderCommand
{
    internal sealed class CreateOrderItemCommandValidator : AbstractValidator<CreateOrderItemRequest>
    {
        public CreateOrderItemCommandValidator()
        {
            RuleFor(oi => oi.ProductId)
                .NotNull()
                .NotEmpty()
                .MaximumLength(DomainConstants.Order.ProductIdMaxLength);

            RuleFor(oi => oi.ProductName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(DomainConstants.Order.ProductNameMaxLength);

            RuleFor(oi => oi.UnitPrice).GreaterThan(0m);
            RuleFor(oi => oi.Quantity).GreaterThan(0);
        }
    }
}
