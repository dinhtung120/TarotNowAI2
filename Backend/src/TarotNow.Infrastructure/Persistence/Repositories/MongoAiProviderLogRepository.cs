/*
 * FILE: MongoAiProviderLogRepository.cs
 * MỤC ĐÍCH: Repository ghi log gọi AI vào MongoDB (collection "ai_provider_logs").
 *   TTL 90 ngày — MongoDB tự xóa document cũ.
 *
 *   CÁC CHỨC NĂNG:
 *   → CreateAsync: ghi 1 log entry sau mỗi lần gọi AI (map DTO → Document → InsertOne)
 *   → GetByUserIdAsync: lấy lịch sử log theo User (phân trang, mới nhất trước)
 *
 *   MAPPING: DTO → Document (khi ghi) và Document → DTO (khi đọc).
 *   Application layer chỉ biết DTO, không biết MongoDB Document tồn tại.
 */

using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IAiProviderLogRepository — ghi/đọc log AI từ MongoDB.
/// Chỉ lưu metadata (token, latency, status), KHÔNG lưu prompt/response raw.
/// </summary>
public class MongoAiProviderLogRepository : IAiProviderLogRepository
{
    private readonly MongoDbContext _mongoContext;

    public MongoAiProviderLogRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    /// <summary>
    /// Ghi 1 log entry sau mỗi lần gọi AI.
    /// Map từ DTO (Application layer) sang Document (Infrastructure layer) rồi insert vào MongoDB.
    /// </summary>
    public async Task CreateAsync(AiProviderLogCreateDto log, CancellationToken cancellationToken = default)
    {
        // Map DTO → MongoDB Document
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

        // InsertOneAsync: thêm 1 document vào collection ai_provider_logs
        await _mongoContext.AiProviderLogs.InsertOneAsync(doc, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Lấy lịch sử log AI theo User, phân trang, mới nhất trước.
    /// Trả về tuple: (danh sách DTO, tổng số document) — UI dùng totalCount cho pagination.
    /// </summary>
    public async Task<(IEnumerable<AiProviderLogDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var userIdStr = userId.ToString();
        // Filter: chỉ lấy document của User này
        var filter = Builders<AiProviderLogDocument>.Filter.Eq(a => a.UserId, userIdStr);

        // Đếm tổng (cho pagination)
        var totalCount = await _mongoContext.AiProviderLogs.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        // Lấy dữ liệu trang hiện tại
        var docs = await _mongoContext.AiProviderLogs
            .Find(filter)
            .SortByDescending(a => a.CreatedAt) // Mới nhất trước
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        // Map Document → DTO (Application layer không biết MongoDB)
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
