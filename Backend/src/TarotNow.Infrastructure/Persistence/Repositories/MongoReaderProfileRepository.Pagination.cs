using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial truy vấn phân trang cho reader directory.
public partial class MongoReaderProfileRepository
{
    /// <summary>
    /// Lấy danh sách reader profile có phân trang và bộ lọc directory.
    /// Luồng xử lý: chuẩn hóa page/pageSize, đếm tổng theo filter, query docs đã sort ưu tiên status rồi map DTO.
    /// </summary>
    public async Task<(IEnumerable<ReaderProfileDto> Profiles, long TotalCount)> GetPaginatedAsync(
        int page,
        int pageSize,
        string? specialty = null,
        string? status = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 12 : Math.Min(pageSize, 200);
        // Page size mặc định 12 phù hợp UI directory dạng grid.

        var filter = BuildDirectoryFilter(specialty, status, searchTerm);

        var totalCount = await _context.ReaderProfiles.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var docs = await QueryDirectoryDocsAsync(filter, normalizedPage, normalizedPageSize, cancellationToken);
        var mappedDocs = docs.Select(doc => BsonSerializer.Deserialize<ReaderProfileDocument>(doc));
        return (mappedDocs.Select(ToDto).ToList(), totalCount);
    }

    /// <summary>
    /// Query raw BSON docs cho directory với status_priority tạm thời.
    /// Luồng xử lý: aggregate match + addFields status_priority + sort + paging rồi project bỏ trường tạm.
    /// </summary>
    private async Task<List<BsonDocument>> QueryDirectoryDocsAsync(
        FilterDefinition<ReaderProfileDocument> filter,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var statusPriority = BuildStatusPriorityExpression();

        return await _context.ReaderProfiles
            .Aggregate()
            .Match(filter)
            .AppendStage<BsonDocument>(new BsonDocument("$addFields", new BsonDocument("status_priority", statusPriority)))
            .AppendStage<BsonDocument>(new BsonDocument("$sort", new BsonDocument
            {
                { "status_priority", 1 },
                { "updated_at", -1 }
            }))
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .AppendStage<BsonDocument>(new BsonDocument("$project", new BsonDocument("status_priority", 0)))
            .As<BsonDocument>()
            .ToListAsync(cancellationToken);
        // Dùng aggregate để sort theo priority tùy biến mà Find/Sort thuần khó biểu diễn gọn.
    }
}
