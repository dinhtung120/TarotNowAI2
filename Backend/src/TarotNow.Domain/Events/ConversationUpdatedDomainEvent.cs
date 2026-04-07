namespace TarotNow.Domain.Events;

public class ConversationUpdatedDomainEvent : IDomainEvent
{
    public string ConversationId { get; }
    public string Type { get; }
    public DateTime OccurredAtUtc { get; }

    public ConversationUpdatedDomainEvent(string conversationId, string type, DateTime occurredAtUtc)
    {
        ConversationId = conversationId;
        Type = type;
        OccurredAtUtc = occurredAtUtc;
    }
}
