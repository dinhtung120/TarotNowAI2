namespace TarotNow.Api.Realtime;

public sealed class NoOpRealtimeDedupStore : IRealtimeDedupStore
{
    public Task<bool> TryClaimAsync(string eventId, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}
