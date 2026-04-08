using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract điểm danh hằng ngày để quản lý trạng thái nhận thưởng và thống kê chuỗi ngày.
public interface IDailyCheckinRepository
{
    /// <summary>
    /// Kiểm tra người dùng đã điểm danh trong ngày nghiệp vụ hay chưa để chặn nhận thưởng trùng.
    /// Luồng xử lý: đối chiếu userId với businessDate và trả true khi đã có bản ghi.
    /// </summary>
    Task<bool> HasCheckedInAsync(string userId, string businessDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ghi nhận điểm danh mới kèm phần thưởng để khóa trạng thái nhận quà trong ngày.
    /// Luồng xử lý: tạo bản ghi check-in theo userId/businessDate và lưu goldReward tương ứng.
    /// </summary>
    Task InsertAsync(string userId, string businessDate, long goldReward, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số lần điểm danh trong khoảng ngày gần đây để tính chuỗi hoặc nhiệm vụ.
    /// Luồng xử lý: lọc theo userId và recentDays, sau đó trả tổng số ngày đã check-in.
    /// </summary>
    Task<int> GetCheckinCountAsync(string userId, int recentDays, CancellationToken cancellationToken = default);
}
