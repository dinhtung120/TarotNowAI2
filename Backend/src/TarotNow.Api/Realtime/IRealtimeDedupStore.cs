namespace TarotNow.Api.Realtime;

public interface IRealtimeDedupStore
{
    Task<bool> TryClaimAsync(string eventId, TimeSpan ttl, CancellationToken cancellationToken = default);
}
