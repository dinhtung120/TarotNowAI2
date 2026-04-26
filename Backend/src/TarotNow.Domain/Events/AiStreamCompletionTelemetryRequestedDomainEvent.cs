namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu ghi telemetry completion cho AI stream theo cơ chế bất đồng bộ.
/// </summary>
public sealed class AiStreamCompletionTelemetryRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// User phát sinh request AI stream.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Định danh request AI để correlation telemetry.
    /// </summary>
    public Guid AiRequestId { get; init; }

    /// <summary>
    /// Session ref liên quan request (nếu có).
    /// </summary>
    public string? SessionId { get; init; }

    /// <summary>
    /// Request id của provider/đối tác (nếu có).
    /// </summary>
    public string? RequestId { get; init; }

    /// <summary>
    /// Số token input gửi lên provider.
    /// </summary>
    public int InputTokens { get; init; }

    /// <summary>
    /// Số token output trả về.
    /// </summary>
    public int OutputTokens { get; init; }

    /// <summary>
    /// Độ trễ stream (ms).
    /// </summary>
    public int LatencyMs { get; init; }

    /// <summary>
    /// Trạng thái telemetry (completed/failed).
    /// </summary>
    public string Status { get; init; } = "failed";

    /// <summary>
    /// Mã lỗi telemetry (nếu có).
    /// </summary>
    public string? ErrorCode { get; init; }

    /// <summary>
    /// Phiên bản prompt áp dụng cho request.
    /// </summary>
    public string? PromptVersion { get; init; }

    /// <summary>
    /// Thời điểm phát sinh event.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
