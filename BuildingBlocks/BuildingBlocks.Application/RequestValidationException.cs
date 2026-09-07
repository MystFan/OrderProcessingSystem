using BuildingBlocks.Domain.Exceptions;

namespace BuildingBlocks.Application
{
    public class RequestValidationException : DomainException
    {
        public RequestValidationException(IDictionary<string, string> errors) : base("Request data is invalid.")
        {
            AdditionalData.ValidationErrors = errors;
        }
    }
}
