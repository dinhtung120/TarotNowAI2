using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// MongoDB implementation cho IAiProviderLogRepository.
///
/// Ghi log mọi AI call (OpenAI, Grok, etc.) vào MongoDB.
/// TTL 90 ngày — MongoDB tự động xóa document cũ.
/// Chỉ metadata (tokens, latency, status), KHÔNG lưu prompt/response raw.
/// </summary>
public class MongoAiProviderLogRepository : IAiProviderLogRepository
{
    private readonly MongoDbContext _mongoContext;

    public MongoAiProviderLogRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>Ghi 1 log entry sau mỗi AI request.</summary>
    public async Task CreateAsync(AiProviderLogCreateDto log, CancellationToken cancellationToken = default)
    {
        var doc = new AiProviderLogDocument
        {
            UserId = log.UserId.ToString(),
            ReadingRef = log.ReadingRef,
            AiRequestRef = log.AiRequestRef,
            Model = log.Model,
            Tokens = new TokenUsage
            {
                InputTokens = log.InputTokens,
                OutputTokens = log.OutputTokens
            },
            LatencyMs = log.LatencyMs,
            PromptVersion = log.PromptVersion,
            Status = log.Status,
            ErrorCode = log.ErrorCode,
            TraceId = log.TraceId,
            CreatedAt = DateTime.UtcNow
        };

        await _mongoContext.AiProviderLogs.InsertOneAsync(doc, cancellationToken: cancellationToken);
    }

    /// <summary>Lấy logs theo user — phân trang, mới nhất trước.</summary>
    public async Task<(IEnumerable<AiProviderLogDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var userIdStr = userId.ToString();
        var filter = Builders<AiProviderLogDocument>.Filter.Eq(a => a.UserId, userIdStr);

        var totalCount = await _mongoContext.AiProviderLogs.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _mongoContext.AiProviderLogs
            .Find(filter)
            .SortByDescending(a => a.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        var dtos = docs.Select(d => new AiProviderLogDto
        {
            Id = d.Id,
            UserId = userId,
            Model = d.Model,
            InputTokens = d.Tokens.InputTokens,
            OutputTokens = d.Tokens.OutputTokens,
            LatencyMs = d.LatencyMs,
            Status = d.Status,
            ErrorCode = d.ErrorCode,
            CreatedAt = d.CreatedAt
        });

        return (dtos, totalCount);
    }
}
