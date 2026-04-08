using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

// Contract điều phối mở phiên đọc bài trả phí để gom nhiều thao tác nghiệp vụ thành một luồng nhất quán.
public interface IReadingSessionOrchestrator
{
    /// <summary>
    /// Bắt đầu phiên đọc bài trả phí và trả kết quả thành công/thất bại có thông điệp lỗi.
    /// Luồng xử lý: nhận request phiên trả phí, thực thi các bước trừ phí/khởi tạo phiên, rồi trả trạng thái cuối.
    /// </summary>
    Task<(bool Success, string ErrorMessage)> StartPaidSessionAsync(
        StartPaidSessionRequest request,
        CancellationToken cancellationToken = default);
}

// Request mở phiên đọc bài trả phí để truyền đủ dữ liệu đầu vào cho orchestrator.
public sealed class StartPaidSessionRequest
{
    // Định danh người dùng mở phiên.
    public Guid UserId { get; init; }

    // Loại trải bài dùng để tính phí và điều phối rule.
    public string SpreadType { get; init; } = string.Empty;

    // Entity phiên đọc bài đã được khởi tạo metadata ban đầu.
    public ReadingSession Session { get; init; } = null!;

    // Chi phí Gold cần trừ khi bắt đầu phiên.
    public long CostGold { get; init; }

    // Chi phí Diamond cần trừ khi bắt đầu phiên.
    public long CostDiamond { get; init; }
}
