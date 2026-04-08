using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract xử lý tích điểm gamification theo các sự kiện hành vi chính.
public interface IGamificationService
{
    /// <summary>
    /// Xử lý hậu sự kiện hoàn tất đọc bài để cộng tiến độ nhiệm vụ/thành tựu liên quan.
    /// Luồng xử lý: nhận userId, đánh giá rule gamification tương ứng và cập nhật điểm/trạng thái.
    /// </summary>
    Task OnReadingCompletedAsync(Guid userId, CancellationToken ct);

    /// <summary>
    /// Xử lý sự kiện check-in để cập nhật chuỗi ngày và phần thưởng tương ứng.
    /// Luồng xử lý: nhận userId/currentStreak rồi áp rule thưởng theo mốc streak hiện tại.
    /// </summary>
    Task OnCheckInAsync(Guid userId, int currentStreak, CancellationToken ct);

    /// <summary>
    /// Xử lý sự kiện tạo bài viết để cộng tiến độ nhiệm vụ cộng đồng.
    /// Luồng xử lý: nhận userId và cập nhật các chỉ số gamification phụ thuộc hành vi đăng bài.
    /// </summary>
    Task OnPostCreatedAsync(Guid userId, CancellationToken ct);
}
