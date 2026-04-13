namespace TarotNow.Api.Realtime;

/// <summary>
/// Nguồn nhận realtime envelope cho bridge SignalR.
/// </summary>
public interface IRealtimeBridgeSource
{
    /// <summary>
    /// Cho biết source có khả dụng để nhận sự kiện realtime hay không.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Subscribe một channel và nhận payload JSON thô.
    /// </summary>
    Task SubscribeAsync(
        string channel,
        Func<string, string, Task> onMessageAsync,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Unsubscribe một channel đã đăng ký trước đó.
    /// </summary>
    Task UnsubscribeAsync(string channel, CancellationToken cancellationToken = default);
}
