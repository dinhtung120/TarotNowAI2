namespace TarotNow.Infrastructure.Events;

public sealed record OutboxEventContract(string Name, int Version, Type EventType)
{
    public string StoredValue => $"{Name}.v{Version}";
}
