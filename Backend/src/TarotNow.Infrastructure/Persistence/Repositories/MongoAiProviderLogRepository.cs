

using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository lưu và truy vấn log gọi AI provider trên Mongo.
public class MongoAiProviderLogRepository : IAiProviderLogRepository
{
    // Mongo context truy cập collection ai_provider_logs.
    private readonly MongoDbContext _mongoContext;

    /// <summary>
    /// Khởi tạo repository AI provider logs.
    /// Luồng xử lý: nhận MongoDbContext từ DI để dùng chung cấu hình kết nối/index.
    /// </summary>
    public MongoAiProviderLogRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>
    /// Tạo bản ghi log AI provider.
    /// Luồng xử lý: map DTO sang document Mongo rồi insert để phục vụ audit và thống kê chi phí.
    /// </summary>
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
        // Ghi CreatedAt tại thời điểm insert để đồng bộ timeline log theo UTC.
    }

    /// <summary>
    /// Lấy danh sách log AI theo user có phân trang.
    /// Luồng xử lý: chuẩn hóa page/pageSize, lọc theo user_id dạng string, đếm tổng rồi lấy page mới nhất trước.
    /// </summary>
    public async Task<(IEnumerable<AiProviderLogDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);
        // Chặn page size quá lớn để tránh truy vấn nặng ở màn hình admin.

        var userIdStr = userId.ToString();
        var filter = Builders<AiProviderLogDocument>.Filter.Eq(a => a.UserId, userIdStr);
        // user_id lưu dạng string trong document để đồng bộ với nhiều nguồn phát sinh log.

        var totalCount = await _mongoContext.AiProviderLogs.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _mongoContext.AiProviderLogs
            .Find(filter)
            .SortByDescending(a => a.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);
        // Sort desc để ưu tiên log mới nhất ở đầu danh sách.

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
