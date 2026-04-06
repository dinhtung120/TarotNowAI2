using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Dịch vụ đầu não quán xuyến nhịp tim truy cập liên tục (Streak) của người chơi.
/// Gọi khi khách hoàn thành giải bài, hay dùng Backgroun-job để chặt rụng chuỗi.
/// </summary>
public interface IStreakService
{
    /// <summary>
    /// Kích hoạt lửa Streak lên mỗi khi AI Stream nhả ra kết quả thành công rực rỡ, đánh trúng điểm "Rút bài hợp lệ".
    /// </summary>
    Task IncrementStreakOnValidDrawAsync(Guid userId, CancellationToken cancellationToken = default);
}
