namespace TarotNow.Api.Realtime;

/// <summary>
/// Source realtime no-op khi không có Redis.
/// </summary>
public sealed class NoOpRealtimeBridgeSource : IRealtimeBridgeSource
{
    /// <inheritdoc />
    public bool IsEnabled => false;

    /// <inheritdoc />
    public Task SubscribeAsync(
        string channel,
        Func<string, string, Task> onMessageAsync,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UnsubscribeAsync(string channel, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
