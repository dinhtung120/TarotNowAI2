using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository abstraction cho collection reader_requests (MongoDB).
///
/// Tại sao dùng ReaderRequestDto thay vì MongoDocument?
/// → Clean Architecture: Application layer không biết về Infrastructure.
/// → Repository implementation (Infrastructure) chịu trách nhiệm map DTO ↔ MongoDocument.
/// → Tương tự cách IReadingSessionRepository dùng ReadingSession entity.
/// </summary>
public interface IReaderRequestRepository
{
    /// <summary>Tạo mới đơn xin Reader.</summary>
    Task AddAsync(ReaderRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>Lấy đơn theo ObjectId.</summary>
    Task<ReaderRequestDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy đơn mới nhất của user (chưa bị xóa).
    /// Dùng để kiểm tra user có đang pending hay không trước khi submit mới.
    /// </summary>
    Task<ReaderRequestDto?> GetLatestByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách đơn có phân trang — dùng cho Admin approval queue.
    /// </summary>
    Task<(IEnumerable<ReaderRequestDto> Requests, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default);

    /// <summary>Cập nhật đơn (status, admin_note, reviewed_by, reviewed_at).</summary>
    Task UpdateAsync(ReaderRequestDto request, CancellationToken cancellationToken = default);
}
