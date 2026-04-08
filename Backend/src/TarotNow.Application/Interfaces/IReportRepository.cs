
using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract quản lý báo cáo vi phạm để đội vận hành xử lý moderation theo quy trình.
public interface IReportRepository
{
    /// <summary>
    /// Tạo báo cáo vi phạm mới khi người dùng gửi phản ánh nội dung.
    /// Luồng xử lý: persist report đầu vào vào kho dữ liệu moderation.
    /// </summary>
    Task AddAsync(ReportDto report, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy báo cáo theo id để xem chi tiết và xử lý quyết định.
    /// Luồng xử lý: truy vấn theo reportId và trả null nếu không tồn tại.
    /// </summary>
    Task<ReportDto?> GetByIdAsync(string reportId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách báo cáo có phân trang và bộ lọc để phục vụ màn hình quản trị.
    /// Luồng xử lý: áp filter trạng thái/đối tượng, phân trang rồi trả items + total.
    /// </summary>
    Task<(IEnumerable<ReportDto> Items, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null, string? targetType = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Chốt kết quả xử lý báo cáo và lưu người xử lý để phục vụ audit.
    /// Luồng xử lý: cập nhật status/result/resolvedBy/adminNote theo reportId và trả kết quả thành công.
    /// </summary>
    Task<bool> ResolveAsync(
        string reportId,
        string status,
        string result,
        string resolvedBy,
        string? adminNote,
        CancellationToken cancellationToken = default);
}
