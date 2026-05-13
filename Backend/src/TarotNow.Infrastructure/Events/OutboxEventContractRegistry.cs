using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.Events;

public static class OutboxEventContractRegistry
{
    private static readonly IReadOnlyDictionary<Type, OutboxEventContract> ByType =
        typeof(IDomainEvent)
            .Assembly
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && typeof(IDomainEvent).IsAssignableFrom(type))
            .Select(type => new OutboxEventContract(ToContractName(type), 1, type))
            .ToDictionary(contract => contract.EventType, contract => contract);

    private static readonly IReadOnlyDictionary<string, OutboxEventContract> ByStoredValue = ByType.Values
        .ToDictionary(contract => contract.StoredValue, contract => contract, StringComparer.OrdinalIgnoreCase);

    public static OutboxEventContract GetByType(Type eventType)
    {
        if (ByType.TryGetValue(eventType, out var contract))
        {
            return contract;
        }

        throw new InvalidOperationException(
            $"Domain event type '{eventType.FullName}' is missing an outbox contract registration.");
    }

    public static bool TryGetByStoredValue(string storedValue, out OutboxEventContract contract)
    {
        return ByStoredValue.TryGetValue(storedValue, out contract!);
    }

    private static string ToContractName(Type eventType)
    {
        var name = eventType.Name;
        const string suffix = "DomainEvent";
        if (name.EndsWith(suffix, StringComparison.Ordinal))
        {
            name = name[..^suffix.Length];
        }

        return string.Concat(name.Select((character, index) =>
            index > 0 && char.IsUpper(character)
                ? "." + char.ToLowerInvariant(character)
                : char.ToLowerInvariant(character).ToString()));
    }
}
