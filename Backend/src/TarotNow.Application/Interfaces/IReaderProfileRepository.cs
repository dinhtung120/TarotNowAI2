

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract hồ sơ reader để quản lý thông tin hiển thị và trạng thái hoạt động của reader.
public interface IReaderProfileRepository
{
    /// <summary>
    /// Tạo hồ sơ reader mới sau khi yêu cầu trở thành reader được duyệt.
    /// Luồng xử lý: persist profile đầu vào thành bản ghi hồ sơ reader.
    /// </summary>
    Task AddAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy hồ sơ reader theo userId để hiển thị hoặc cập nhật thông tin cá nhân.
    /// Luồng xử lý: truy vấn theo userId và trả null nếu chưa có hồ sơ.
    /// </summary>
    Task<ReaderProfileDto?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy nhiều hồ sơ reader theo danh sách userId để tối ưu truy vấn danh sách.
    /// Luồng xử lý: batch query theo tập userIds và trả danh sách profile tương ứng.
    /// </summary>
    Task<IEnumerable<ReaderProfileDto>> GetByUserIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật hồ sơ reader khi thay đổi bio, chuyên môn hoặc trạng thái hiển thị.
    /// Luồng xử lý: ghi đè dữ liệu profile theo userId của reader.
    /// </summary>
    Task UpdateAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa hồ sơ reader theo userId khi tài khoản không còn vai trò reader.
    /// Luồng xử lý: tìm và loại bỏ bản ghi profile gắn với userId tương ứng.
    /// </summary>
    Task DeleteByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách hồ sơ reader có phân trang và bộ lọc để phục vụ màn hình khám phá reader.
    /// Luồng xử lý: áp filter specialty/status/searchTerm, phân trang rồi trả profiles + total.
    /// </summary>
    Task<(IEnumerable<ReaderProfileDto> Profiles, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize,
        string? specialty = null, string? status = null, string? searchTerm = null,
        CancellationToken cancellationToken = default);
}
