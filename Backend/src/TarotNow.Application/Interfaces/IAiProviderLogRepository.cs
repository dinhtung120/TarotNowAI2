namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository interface cho ai_provider_logs — ghi log AI calls.
///
/// Mục đích: Audit trail cho mọi lần gọi AI provider (OpenAI, Grok, etc.)
/// Dữ liệu tự động xóa sau 90 ngày (TTL index trong MongoDB).
/// </summary>
public interface IAiProviderLogRepository
{
    /// <summary>
    /// Ghi 1 log entry — gọi sau mỗi AI request (thành công hay thất bại).
    /// </summary>
    Task CreateAsync(AiProviderLogCreateDto log, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy logs theo user — phân trang, sắp xếp mới nhất trước.
    /// Dùng trong admin panel để debug AI issues.
    /// </summary>
    Task<(IEnumerable<AiProviderLogDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
}

/// <summary>DTO để tạo mới AI provider log.</summary>
public class AiProviderLogCreateDto
{
    public Guid UserId { get; set; }
    public string? ReadingRef { get; set; }
    public string? AiRequestRef { get; set; }
    public string Model { get; set; } = string.Empty;
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public int LatencyMs { get; set; }
    public string? PromptVersion { get; set; }
    public string Status { get; set; } = "requested";
    public string? ErrorCode { get; set; }
    public string? TraceId { get; set; }
}

/// <summary>DTO đọc AI provider log.</summary>
public class AiProviderLogDto
{
    public string Id { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Model { get; set; } = string.Empty;
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public int LatencyMs { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public DateTime CreatedAt { get; set; }
}
