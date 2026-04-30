namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract xử lý một vòng dispatch outbox batch.
/// </summary>
public interface IOutboxBatchProcessor
{
    /// <summary>
    /// Xử lý một lần: claim batch, publish MediatR notifications, cập nhật trạng thái retry/dead-letter.
    /// </summary>
    Task<int> ProcessOnceAsync(CancellationToken cancellationToken = default);
}
