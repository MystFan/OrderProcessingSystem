using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace BuildingBlocks.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            foreach (IValidator<TRequest> validator in validators)
            {
                ValidationResult result = await validator.ValidateAsync(context, cancellationToken);

                Dictionary<string, string> errors = result.Errors.ToDictionary(error => error.PropertyName, error => error.ErrorMessage);

                if (result.Errors.Count > 0)
                {
                    throw new RequestValidationException(errors);
                }
            }

            return await next();
        }
    }
}
