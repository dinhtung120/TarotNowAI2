using MediatR;
using System;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

// Command khởi tạo phiên reading mới và tính chi phí theo spread/currency.
public class InitReadingSessionCommand : IRequest<InitReadingSessionResult>
{
    // Định danh user khởi tạo phiên đọc bài.
    public Guid UserId { get; set; }

    // Loại spread cần mở phiên.
    public string SpreadType { get; set; } = string.Empty;

    // Câu hỏi người dùng gửi kèm phiên reading (tùy chọn).
    public string? Question { get; set; }

    // Đơn vị tiền sử dụng để thanh toán (gold/diamond).
    public string Currency { get; set; } = string.Empty;
}

// DTO kết quả khởi tạo phiên reading.
public class InitReadingSessionResult
{
    // Định danh session vừa được mở.
    public string SessionId { get; set; } = string.Empty;

    // Chi phí gold áp dụng cho phiên.
    public long CostGold { get; set; }

    // Chi phí diamond áp dụng cho phiên.
    public long CostDiamond { get; set; }
}
