/*
 * ===================================================================
 * FILE: GetEscrowStatusQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Escrow.Queries.GetEscrowStatus
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh Trích Xuất Hồ Sơ Giao Dịch (Finance Session) của một Box Chat.
 *
 * TẦM QUAN TRỌNG VỀ UI:
 *   Khi Khách hàng mở Box Chat lên, Frontend bắt buộc phải gọi API này 
 *   để vẽ các cục thông báo "Đã Đặt Cọc 100 Kim Cương, Đang chờ Thợ trả lời".
 *   Nếu Thợ đã trả lời -> Đổi màu thông báo, hiện nút "Release" và "Dispute".
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Escrow.Queries.GetEscrowStatus;

/// <summary>
/// Gói Lệnh: Đòi xem Trạng Thái Tiền Bạc trong 1 Box Chat cụ thể.
/// </summary>
public class GetEscrowStatusQuery : IRequest<EscrowStatusResult?>
{
    /// <summary>Khoá chính của Phòng Trò Chuyện.</summary>
    public string ConversationRef { get; set; } = string.Empty;
    
    /// <summary>Biển số Định Danh người Bấm Lệnh để đối chiếu Phân Quyền.</summary>
    public Guid RequesterId { get; set; }
}

/// <summary>
/// Bộ Sổ Xương Sống: Chứa Tổng Kết của Đại Gia Đình Các Khoản Cọc/Giao Dịch
/// Tương tự như bảng sao kê của 1 vòng đời "Bói quẻ".
/// </summary>
public class EscrowStatusResult
{
    public Guid SessionId { get; set; }
    
    /// <summary>Active, Closed, hay Disputed (Đang Rầm Rĩ Kiện Tụng).</summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>Tổng thiệt hại kim cương đã bị Đóng Băng. Tiền càng nhiều Áp lực càng lớn.</summary>
    public long TotalFrozen { get; set; }
    
    /// <summary>Danh sách Câu Hỏi (Ví dụ: Câu 1 = Tình Yêu, Câu 2 = Sự Nghiệp - Add Question).</summary>
    public List<QuestionItemResult> Items { get; set; } = new();
}

/// <summary>
/// Thẻ Ghi Chú (Line Item) đại diện cho 1 Câu Hỏi (Question) mà User đã đặt Cọc.
/// Cung cấp toàn bộ Mốc thời gian để Frontend vẽ Quá trình (Timeline/Progress Bar).
/// </summary>
public class QuestionItemResult
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty; // Main hoặc AddQuestion
    public long AmountDiamond { get; set; }
    public string Status { get; set; } = string.Empty;
    
    // Hệ Sinh Thái Các Mốc Thời Gian Quyết Định Sinh Tử Giao Dịch
    public DateTime? AcceptedAt { get; set; }  // Lúc rớt tiền cọc.
    public DateTime? RepliedAt { get; set; }   // Lúc Thầy nhả bùa.
    public DateTime? AutoRefundAt { get; set; } // Ngày tháng Tự Động Hoàn Tiền (Vì Thầy Lặn Mất Tăm).
    public DateTime? AutoReleaseAt { get; set; } // Ngày tháng Tự Chuyển Tiền Cho Thầy (Khách lặn mất tăm 24H).
    public DateTime? ReleasedAt { get; set; } // Giờ phút Giải ngân xong.
    public DateTime? DisputeWindowEnd { get; set; } // Hạn chót Bấm nút Kiện cáo.
}

public class GetEscrowStatusQueryHandler : IRequestHandler<GetEscrowStatusQuery, EscrowStatusResult?>
{
    private readonly IChatFinanceRepository _financeRepo;

    public GetEscrowStatusQueryHandler(IChatFinanceRepository financeRepo) => _financeRepo = financeRepo;

    public async Task<EscrowStatusResult?> Handle(GetEscrowStatusQuery req, CancellationToken ct)
    {
        // 1. Gõ cửa Database qua Repo. Lấy Session Bố.
        var session = await _financeRepo.GetSessionByConversationRefAsync(req.ConversationRef, ct);
        
        // Nếu Bố chưa tồn tại (tức là 2 người đang chat tán gẫu chưa có Hóa đơn Báo giá) -> Trả Không (Null).
        if (session == null) return null;

        // -------------------------------------------------------------
        //  2. LƯỚI AN NINH BẢO MẬT (AUTHORIZATION GATE)
        //  Chỉ người trong cuộc (User Hoặc Reader) mới được xem Phơi Tiền.
        //  Tuyệt đối Không Cho Phép User khác truyền lụi ConversationRef để đào sâu thông tin tài chính người lạ!
        // -------------------------------------------------------------
        if (session.UserId != req.RequesterId && session.ReaderId != req.RequesterId)
            return null;

        // 3. Đã qua cửa, Gọi các Item Con ra.
        var items = await _financeRepo.GetItemsBySessionIdAsync(session.Id, ct);

        // 4. Bơm Data vào Cỗ xe DTO gửi ngược về Frontend.
        return new EscrowStatusResult
        {
            SessionId = session.Id,
            Status = session.Status,
            TotalFrozen = session.TotalFrozen,
            Items = items.Select(i => new QuestionItemResult
            {
                Id = i.Id,
                Type = i.Type,
                AmountDiamond = i.AmountDiamond,
                Status = i.Status,
                AcceptedAt = i.AcceptedAt,
                RepliedAt = i.RepliedAt,
                AutoRefundAt = i.AutoRefundAt,
                AutoReleaseAt = i.AutoReleaseAt,
                ReleasedAt = i.ReleasedAt,
                DisputeWindowEnd = i.DisputeWindowEnd,
            }).ToList()
        };
    }
}
