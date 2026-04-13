using MediatR;

namespace TarotNow.Application.Features.Admin.Queries.GetOutboxDashboard;

/// <summary>
/// Query lấy dashboard vận hành outbox cho admin.
/// </summary>
public sealed class GetOutboxDashboardQuery : IRequest<OutboxDashboardDto>
{
    /// <summary>
    /// Số lượng bản ghi chi tiết trả về cho mỗi nhóm top.
    /// </summary>
    public int Top { get; set; } = 20;
}

/// <summary>
/// DTO tổng hợp trạng thái vận hành outbox.
/// </summary>
public sealed class OutboxDashboardDto
{
    /// <summary>
    /// Số message đang pending.
    /// </summary>
    public int PendingCount { get; init; }

    /// <summary>
    /// Số message đang processing.
    /// </summary>
    public int ProcessingCount { get; init; }

    /// <summary>
    /// Số message failed và đang chờ retry.
    /// </summary>
    public int FailedCount { get; init; }

    /// <summary>
    /// Số message đã vào dead-letter.
    /// </summary>
    public int DeadLetterCount { get; init; }

    /// <summary>
    /// Số message failed đã quá hạn retry.
    /// </summary>
    public int RetryOverdueCount { get; init; }

    /// <summary>
    /// Tuổi message pending lâu nhất (giây).
    /// </summary>
    public long OldestPendingAgeSeconds { get; init; }

    /// <summary>
    /// Tuổi message failed lâu nhất (giây).
    /// </summary>
    public long OldestFailedAgeSeconds { get; init; }

    /// <summary>
    /// Tuổi message dead-letter lâu nhất (giây).
    /// </summary>
    public long OldestDeadLetterAgeSeconds { get; init; }

    /// <summary>
    /// Retry age lớn nhất trong failed/dead-letter (giây).
    /// </summary>
    public long MaxRetryAgeSeconds { get; init; }

    /// <summary>
    /// Danh sách failed message nổi bật.
    /// </summary>
    public IReadOnlyList<OutboxMessageSummaryDto> TopFailed { get; init; } = [];

    /// <summary>
    /// Danh sách dead-letter message nổi bật.
    /// </summary>
    public IReadOnlyList<OutboxMessageSummaryDto> TopDeadLetters { get; init; } = [];
}

/// <summary>
/// DTO tóm tắt một outbox message cho dashboard vận hành.
/// </summary>
public sealed class OutboxMessageSummaryDto
{
    /// <summary>
    /// Định danh outbox message.
    /// </summary>
    public Guid MessageId { get; init; }

    /// <summary>
    /// Tên CLR type của event.
    /// </summary>
    public string EventType { get; init; } = string.Empty;

    /// <summary>
    /// Số lần đã attempt dispatch.
    /// </summary>
    public int AttemptCount { get; init; }

    /// <summary>
    /// Mốc retry kế tiếp.
    /// </summary>
    public DateTime NextAttemptAtUtc { get; init; }

    /// <summary>
    /// Mốc tạo message.
    /// </summary>
    public DateTime CreatedAtUtc { get; init; }

    /// <summary>
    /// Retry age theo công thức giám sát (giây).
    /// </summary>
    public long RetryAgeSeconds { get; init; }

    /// <summary>
    /// Trích đoạn lỗi gần nhất để quan sát nhanh.
    /// </summary>
    public string? LastErrorPreview { get; init; }
}
