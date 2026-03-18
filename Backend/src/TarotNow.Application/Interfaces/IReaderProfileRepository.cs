using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository abstraction cho collection reader_profiles (MongoDB).
///
/// Dùng ReaderProfileDto (Application layer) thay vì ReaderProfileDocument (Infrastructure).
/// Repository implementation chịu trách nhiệm map DTO ↔ MongoDocument.
/// </summary>
public interface IReaderProfileRepository
{
    /// <summary>Tạo profile mới — gọi khi admin approve reader request.</summary>
    Task AddAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default);

    /// <summary>Lấy profile theo userId (UUID string từ PostgreSQL users).</summary>
    Task<ReaderProfileDto?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>Cập nhật profile (bio, pricing, specialties, status).</summary>
    Task UpdateAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default);

    /// <summary>Soft delete profile theo userId (dùng cho compensation).</summary>
    Task DeleteByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách Reader có phân trang với bộ lọc.
    /// Chỉ trả về readers chưa bị soft delete.
    /// </summary>
    Task<(IEnumerable<ReaderProfileDto> Profiles, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize,
        string? specialty = null, string? status = null, string? searchTerm = null,
        CancellationToken cancellationToken = default);
}
