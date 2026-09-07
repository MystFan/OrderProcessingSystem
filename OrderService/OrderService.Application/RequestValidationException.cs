using BuildingBlocks.Domain.Exceptions;
using OrderService.Domain.Exceptions;

namespace OrderService.Application
{
    public class RequestValidationException : OrderServiceDomainException
    {
        public RequestValidationException(IDictionary<string, string> errors) : base(ExceptionReasonCode.InvalidRequest, "Request data is invalid.")
        {
            AdditionalData.ValidationErrors = errors;
        }
    }
}
