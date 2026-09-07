using BuildingBlocks.Application.Abstract.Command;
using FluentValidation;
using OrderService.Domain;

namespace OrderService.Application.Commands.CreateOrderCommand
{
    public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>, IFluentValidator
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(c => c.CustomerId)
                .NotNull()
                .NotEmpty()
                .MaximumLength(DomainConstants.Order.CustomerIdMaxLength);

            RuleForEach(c => c.Items)
                .SetValidator(new CreateOrderItemCommandValidator());
        }
    }
}
