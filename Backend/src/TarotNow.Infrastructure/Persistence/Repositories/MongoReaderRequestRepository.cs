

using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý yêu cầu đăng ký reader trên Mongo.
public class MongoReaderRequestRepository : IReaderRequestRepository
{
    // Mongo context truy cập collection reader_requests.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository reader request.
    /// Luồng xử lý: nhận MongoDbContext từ DI để thao tác dữ liệu yêu cầu.
    /// </summary>
    public MongoReaderRequestRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Thêm mới yêu cầu đăng ký reader.
    /// Luồng xử lý: map DTO sang document, insert Mongo và gán id phát sinh lại cho DTO.
    /// </summary>
    public async Task AddAsync(ReaderRequestDto request, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(request);
        await _context.ReaderRequests.InsertOneAsync(doc, cancellationToken: cancellationToken);
        request.Id = doc.Id;
        // Giữ đồng bộ id để caller phản hồi lại client chính xác.
    }

    /// <summary>
    /// Lấy yêu cầu theo id nếu chưa bị xóa mềm.
    /// Luồng xử lý: filter id + is_deleted=false rồi map DTO.
    /// </summary>
    public async Task<ReaderRequestDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderRequestDocument>.Filter.And(
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.Id, id),
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.IsDeleted, false)
        );

        var doc = await _context.ReaderRequests.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Lấy yêu cầu mới nhất của một user.
    /// Luồng xử lý: filter theo userId + chưa xóa, sort created_at desc và lấy bản ghi đầu.
    /// </summary>
    public async Task<ReaderRequestDto?> GetLatestByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ReaderRequestDocument>.Filter.And(
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.UserId, userId),
            Builders<ReaderRequestDocument>.Filter.Eq(r => r.IsDeleted, false)
        );

        var doc = await _context.ReaderRequests
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        return doc == null ? null : ToDto(doc);
    }

    /// <summary>
    /// Lấy danh sách yêu cầu reader có phân trang và lọc trạng thái.
    /// Luồng xử lý: chuẩn hóa page/pageSize, filter is_deleted=false + status tùy chọn, đếm tổng rồi trả trang mới nhất trước.
    /// </summary>
    public async Task<(IEnumerable<ReaderRequestDto> Requests, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);
        // Chặn page size lớn để tránh tải nặng cho dashboard admin.

        var filterBuilder = Builders<ReaderRequestDocument>.Filter;
        var filter = filterBuilder.Eq(r => r.IsDeleted, false);

        if (!string.IsNullOrEmpty(statusFilter))
        {
            filter = filterBuilder.And(filter, filterBuilder.Eq(r => r.Status, statusFilter));
            // Chỉ lọc status khi caller truyền giá trị cụ thể.
        }

        var totalCount = await _context.ReaderRequests.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _context.ReaderRequests
            .Find(filter)
            .SortByDescending(r => r.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return (docs.Select(ToDto), totalCount);
    }

    /// <summary>
    /// Cập nhật yêu cầu reader.
    /// Luồng xử lý: replace document theo id sau khi gán UpdatedAt hiện tại.
    /// </summary>
    public async Task UpdateAsync(ReaderRequestDto request, CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(request);
        doc.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<ReaderRequestDocument>.Filter.Eq(r => r.Id, doc.Id);
        await _context.ReaderRequests.ReplaceOneAsync(filter, doc, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Map ReaderRequestDto sang document Mongo.
    /// Luồng xử lý: chuẩn hóa id và copy đầy đủ metadata xét duyệt.
    /// </summary>
    private static ReaderRequestDocument ToDocument(ReaderRequestDto dto)
    {
        return new ReaderRequestDocument
        {
            Id = string.IsNullOrEmpty(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
            UserId = dto.UserId,
            Status = dto.Status,
            IntroText = dto.IntroText,
            ProofDocuments = dto.ProofDocuments,
            AdminNote = dto.AdminNote,
            ReviewedBy = dto.ReviewedBy,
            ReviewedAt = dto.ReviewedAt,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }

    /// <summary>
    /// Map ReaderRequestDocument sang DTO.
    /// Luồng xử lý: chuyển đổi trực tiếp dữ liệu phục vụ API quản lý yêu cầu reader.
    /// </summary>
    private static ReaderRequestDto ToDto(ReaderRequestDocument doc)
    {
        return new ReaderRequestDto
        {
            Id = doc.Id,
            UserId = doc.UserId,
            Status = doc.Status,
            IntroText = doc.IntroText,
            ProofDocuments = doc.ProofDocuments,
            AdminNote = doc.AdminNote,
            ReviewedBy = doc.ReviewedBy,
            ReviewedAt = doc.ReviewedAt,
            CreatedAt = doc.CreatedAt,
            UpdatedAt = doc.UpdatedAt
        };
    }
}
