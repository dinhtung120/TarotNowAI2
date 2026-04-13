namespace TarotNow.Application.Interfaces.DomainEvents;

/// <summary>
/// Contract idempotency cho domain event handlers.
/// </summary>
public interface IEventHandlerIdempotencyService
{
    /// <summary>
    /// Kiểm tra handler đã xử lý thành công message hay chưa.
    /// </summary>
    Task<bool> HasProcessedAsync(Guid outboxMessageId, string handlerName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đánh dấu handler đã xử lý thành công message.
    /// </summary>
    Task MarkProcessedAsync(Guid outboxMessageId, string handlerName, CancellationToken cancellationToken = default);
}
