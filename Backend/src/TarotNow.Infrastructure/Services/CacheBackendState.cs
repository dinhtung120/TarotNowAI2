namespace TarotNow.Infrastructure.Services;

public sealed class CacheBackendState
{
    public CacheBackendState(bool usesRedis)
    {
        UsesRedis = usesRedis;
    }

    public bool UsesRedis { get; }
}
