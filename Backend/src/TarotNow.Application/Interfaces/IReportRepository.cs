using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository abstraction cho collection reports (MongoDB).
///
/// Reports = báo cáo vi phạm, admin xử lý qua queue.
/// </summary>
public interface IReportRepository
{
    /// <summary>Tạo báo cáo mới.</summary>
    Task AddAsync(ReportDto report, CancellationToken cancellationToken = default);

    /// <summary>
    /// Danh sách reports phân trang (admin).
    /// Sort by created_at DESC.
    /// </summary>
    Task<(IEnumerable<ReportDto> Items, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default);
}
