using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.Messaging.DomainEvents;

/// <summary>
/// Registry ánh xạ event type name sang CLR type để deserialize outbox payload.
/// </summary>
public static class DomainEventTypeRegistry
{
    private static readonly IReadOnlyDictionary<string, Type> DomainEventTypes =
        typeof(IDomainEvent)
            .Assembly
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && typeof(IDomainEvent).IsAssignableFrom(type))
            .Where(type => string.IsNullOrWhiteSpace(type.FullName) == false)
            .ToDictionary(type => type.FullName!, type => type, StringComparer.Ordinal);

    /// <summary>
    /// Resolve event CLR type theo full name.
    /// </summary>
    public static bool TryResolve(string eventTypeName, out Type eventType)
        => DomainEventTypes.TryGetValue(eventTypeName, out eventType!);
}
