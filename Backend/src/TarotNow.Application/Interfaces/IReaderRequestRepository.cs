

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract yêu cầu trở thành reader để theo dõi tiến trình xét duyệt.
public interface IReaderRequestRepository
{
    /// <summary>
    /// Tạo yêu cầu reader mới khi người dùng nộp hồ sơ đăng ký.
    /// Luồng xử lý: persist request đầu vào thành bản ghi chờ xét duyệt.
    /// </summary>
    Task AddAsync(ReaderRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy yêu cầu reader theo id để phục vụ duyệt hoặc xem chi tiết.
    /// Luồng xử lý: truy vấn theo khóa yêu cầu và trả null nếu không tồn tại.
    /// </summary>
    Task<ReaderRequestDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy yêu cầu reader gần nhất của người dùng để kiểm soát gửi lại hồ sơ.
    /// Luồng xử lý: lọc theo userId, sắp xếp mới nhất và trả yêu cầu gần nhất.
    /// </summary>
    Task<ReaderRequestDto?> GetLatestByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách yêu cầu reader có phân trang để phục vụ màn hình quản trị duyệt hồ sơ.
    /// Luồng xử lý: lọc theo status tùy chọn, áp page/pageSize và trả requests + total.
    /// </summary>
    Task<(IEnumerable<ReaderRequestDto> Requests, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật yêu cầu reader khi thay đổi trạng thái duyệt hoặc ghi chú xử lý.
    /// Luồng xử lý: ghi đè dữ liệu request tương ứng trong kho dữ liệu.
    /// </summary>
    Task UpdateAsync(ReaderRequestDto request, CancellationToken cancellationToken = default);
}
