namespace BuildingBlocks.Messaging
{
    public sealed record MessageEnvelope<T>(MessageMetadata Metadata, T Payload);
}
