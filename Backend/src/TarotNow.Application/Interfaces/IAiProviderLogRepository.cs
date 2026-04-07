namespace TarotNow.Application.Interfaces;

public interface IAiProviderLogRepository
{
        Task CreateAsync(AiProviderLogCreateDto log, CancellationToken cancellationToken = default);

        Task<(IEnumerable<AiProviderLogDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
}

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
