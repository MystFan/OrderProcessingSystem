namespace BuildingBlocks.Messaging
{
    public sealed record MessageMetadata(string MessageId, string? CorrelationId, string? CausationId);
}
