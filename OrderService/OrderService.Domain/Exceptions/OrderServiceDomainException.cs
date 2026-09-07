using BuildingBlocks.Domain.Exceptions;

namespace OrderService.Domain.Exceptions
{
    public class OrderServiceDomainException : DomainException
    {
        public ExceptionReasonCode ReasonCode { get; }

        public OrderServiceDomainException(ExceptionReasonCode reasonCode, string message, Exception? innerException = null)
            : base(message, innerException)
        {
            ReasonCode = reasonCode;
        }
    }
}
