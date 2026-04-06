using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Giao diện kho chứa (Repository) dọn dẹp Document Check-in mỗi ngày của Khách.
/// Ngăn cản tuyệt đối nạn spam nút nhận ngập mồm tiền Gold. 
/// </summary>
public interface IDailyCheckinRepository
{
    /// <summary>
    /// Kiểm kê xem ngày hôm nay khách lấy tờ vé Check-in nhận Vàng chưa?
    /// </summary>
    Task<bool> HasCheckedInAsync(string userId, string businessDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Chốt khoá in mộc xác nhận khách đã lĩnh Gold hôm nay.
    /// </summary>
    Task InsertAsync(string userId, string businessDate, long goldReward, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Tổng kết điểm danh trong N ngày qua (nếu hiển thị biểu đồ check-in trên UX cho vui mắt).
    /// </summary>
    Task<int> GetCheckinCountAsync(string userId, int recentDays, CancellationToken cancellationToken = default);
}
