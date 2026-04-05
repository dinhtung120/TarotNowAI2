namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    private async Task<bool> TryAcquireSignalRateLimitAsync(string operation, Guid userId, TimeSpan window, string rejectMessage)
    {
        var rateLimitKey = $"ratelimit:{operation}:{userId}";
        var allowed = await _cacheService.CheckRateLimitAsync(rateLimitKey, window);
        if (allowed)
        {
            return true;
        }

        await SendClientErrorAsync("too_many_requests", rejectMessage);
        return false;
    }
}
