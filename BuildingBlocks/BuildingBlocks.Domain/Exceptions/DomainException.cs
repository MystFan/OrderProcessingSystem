namespace BuildingBlocks.Domain.Exceptions
{
    public class DomainException : ApplicationException
    {
        public AdditionalData AdditionalData { get; } = new();

        public DomainException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}
