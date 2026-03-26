namespace TarotNow.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredAtUtc { get; }
}
