/*
 * ===================================================================
 * FILE: GetReadingDetailQuery.cs
 * NAMESPACE: TarotNow.Application.Features.History.Queries.GetReadingDetail
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh chuyên bóc tách "Ruột Gan" của một Trải Bài Tarot Cụ Thể.
 *   API này thường được gọi khi User nhấp chuột vào LỊCH SỬ trên Profile 
 *   để Đọc Lại bài luận giải ngày sửa ngày xưa.
 *
 * NOTE UI/UX:
 *   Nó phơi bày cả "Thác Nước" (Timeline) những lần AI trả lời cho User. 
 *   Kể cả các câu Follow Up bắt thêm Phí nhỏ (VD: Mất thêm 5 Kim Cương).
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.History.Queries.GetReadingDetail;

/// <summary>
/// Gói Lệnh: Đòi xem Cốt Truyện Toàn Cảnh Tập Phim Bói Bài này (Mã Phiên SessionId).
/// </summary>
public class GetReadingDetailQuery : IRequest<GetReadingDetailResponse?>
{
    /// <summary>Cái Thẻ Chăn Màn Trấn Cửa (Chỉ chính chủ mới được xem lịch sử đời mình).</summary>
    public Guid UserId { get; set; }
    
    /// <summary>Mã ObjectID Mongo của Bộ Trải Bài.</summary>
    public string SessionId { get; set; } = string.Empty;
}

/// <summary>
/// Bức tranh Chữ Tổng Kết vứt ra cho Frontend múa Đồ họa.
/// </summary>
public class GetReadingDetailResponse
{
    public string Id { get; set; } = string.Empty;
    public string SpreadType { get; set; } = string.Empty;
    
    /// <summary>Gói Chuỗi JSON Thô mô tả những lá bài bốc được. Frontend tự giải phẫu hiển thị.</summary>
    public string? CardsDrawn { get; set; }
    
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // ==========================================
    // CÂU HỎI VÀ TRẢ LỜI CỦA AI VỚI NHAU
    // ==========================================
    
    public string? AiSummary { get; set; }
    public IEnumerable<FollowupDto> Followups { get; set; } = new List<FollowupDto>();

    /// <summary>
    /// Bảng Thống Kê Các Cuốc Chat Với AI.
    /// Dùng cho Phiên Bản MVP hiện tại (Trích xuất từ Collection log AiRequests).
    /// </summary>
    public IEnumerable<AiRequestDto> AiInteractions { get; set; } = new List<AiRequestDto>();
}

public class FollowupDto
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}

/// <summary>
/// Vỏ Bọc (DTO) Thu nhỏ diễn tiến 1 lần hỏi đáp.
/// Chứa Trạng Thái Sinh Mạng API (Stop/Length), và SỐ TIỀN MÁU (ChargeDiamond) đã nộp.
/// </summary>
public class AiRequestDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? FinishReason { get; set; }
    
    /// <summary>Giọt máu khô - Mỗi cú bói đốt bao nhiêu tiền?</summary>
    public long ChargeDiamond { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <summary>Cho biết đây là Lần Bói Khai Phá (InitialReading) hay Lấy Tiền Nhỏ Hỏi Thêm (Followup - Truy Sát Chi Tiết).</summary>
    public string? RequestType { get; set; }
}

public class GetReadingDetailQueryHandler : IRequestHandler<GetReadingDetailQuery, GetReadingDetailResponse?>
{
    private readonly IReadingSessionRepository _readingRepo;

    public GetReadingDetailQueryHandler(IReadingSessionRepository readingRepo)
    {
        _readingRepo = readingRepo;
    }

    public async Task<GetReadingDetailResponse?> Handle(GetReadingDetailQuery request, CancellationToken cancellationToken)
    {
        // Nhờ MongoDB nối chuỗi 2 Collection: Lịch Sử Quẻ Bói + Logs Tiền Sử Call API.
        var session = await _readingRepo.GetSessionWithAiRequestsAsync(request.SessionId, cancellationToken);

        // Kèo này sai mã hoặc xóa rồi -> Biến đi. (Giữ Null để Controller tự bắn lỗi 404 - Unit test check rule này).
        if (!session.HasValue)
            return null; 

        // -------------------------------------------------------------
        //  LƯỚI AN NINH BẢO MẬT GIA PHẢ QUẺ BÓI
        // -------------------------------------------------------------
        // Trừ phi là Tòa án, Không ai được xem Trải bài Tarot của Cô Cô và Dương Quá!
        if (session.Value.ReadingSession.UserId != request.UserId.ToString())
            throw new UnauthorizedAccessException("Reading session not found or access denied");

        // Gom Data Trả về Mâm
        return new GetReadingDetailResponse
        {
            Id = session.Value.ReadingSession.Id,
            SpreadType = session.Value.ReadingSession.SpreadType,
            CardsDrawn = session.Value.ReadingSession.CardsDrawn,
            IsCompleted = session.Value.ReadingSession.IsCompleted,
            CreatedAt = session.Value.ReadingSession.CreatedAt,
            CompletedAt = session.Value.ReadingSession.CompletedAt,
            
            AiSummary = session.Value.ReadingSession.AiSummary,
            Followups = session.Value.ReadingSession.Followups.Select(f => new FollowupDto
            {
                Question = f.Question,
                Answer = f.Answer
            }).ToList(),
            
            // Xử lý Phân Cấp Loại Hình Chat Tóm Tắt (Dựa vào mẹo Check Tiền và Khóa Sinh Tồn IdempotencyKey).
            AiInteractions = session.Value.AiRequests.Select(a => new AiRequestDto
            {
                Id = a.Id,
                Status = a.Status,
                FinishReason = a.FinishReason,
                ChargeDiamond = a.ChargeDiamond,
                CreatedAt = a.CreatedAt,
                
                // MẸO (HACKY TRICK): Nếu Request tiêu tốn > 5 Kim Cương thì đó là câu Trải Đầu Tiên,
                // Còn Rẻ Rúng <= 5 Kim Cương thì chắc chắn là Câu Hỏi Phụ (Follow Up Chat).
                RequestType = a.IdempotencyKey != null && a.IdempotencyKey.Contains("ai_stream") ? 
                              (a.ChargeDiamond > 5 ? "Followup" : "InitialReading") : "Unknown"
            }).OrderBy(x => x.CreatedAt) // Mới nhất lên Màn Ảnh Nhỏ
        };
    }
}
