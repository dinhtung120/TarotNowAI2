/*
 * ===================================================================
 * FILE: AiRequest.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Table: Đại Diện Cho Bảng Lịch Sử Lệnh Gọi AI Của Khách.
 *   Theo Dõi Rõ Chi Phí Bắn API Đi OpenAI, Trạng Thái Thất Bại Xin Lỗi Trả Lại Tiền (Hoàn Tiền Nhờ Biến Môi Trình Idempotency Mốc).
 * ===================================================================
 */

namespace TarotNow.Domain.Entities;

/// <summary>
/// Cấu Trúc Khối Dữ Liệu Ánh Xạ Xuống Bảng SQL `ai_requests` Ở Tầng Postgres.
/// Quản Lý Việc AI Ở Mỹ Chạy Cắn Tiền Có Đúng Hay Chưa, Dòng Trả Lời Trôi Từ Lúc Nào.
/// </summary>
public class AiRequest
{
    // Cột Đinh: Mã Sinh ID Gọi Của MỗI Dòng Giao Dịch Chat AI Này Đầu Tiên.
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Cột Bám Vào Chủ Mưu: Ai Đã Sờ Máy Và Trừ Tiền Căn Tục Của Đứa Đó.
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Sợi Dây Nối Máu Kết Dính Giữa Thế Giới SQL (Tiền) Và Mongo (Lịch Sử Từng Ván Chat Document Bự).
    /// Dài Hiện Đúng 24 Ký Tự Text (Chữ Hex Của MongoDB ObjectId Căn Bản Cột Trỏ Link Này Kéo Xuống Sẽ So Sánh Về Phòng Nào).
    /// </summary>
    public string ReadingSessionRef { get; set; } = null!;
    
    /// <summary>
    /// Đếm Sự Tham Lam Của Khách Hỏi Dai: NULL Tức Lần Bấm Giải Trải Đầu. Bấm Follow-Up Lần 1 Bằng 1 ... Cho Tới 5 Là Phải Đuổi Hủy.
    /// </summary>
    public short? FollowupSequence { get; set; }

    /// <summary>
    /// Tình Trạng Sống Chết Phiên Trải Chat Này Đang Stream Dữ Liệu? Hay Mất Sóng Đứt Gánh, Xong Rồi Thanh Toán Thế Nào Cắt (Trạng Thái: Requested/Completed/Failed).
    /// </summary>
    public string Status { get; set; } = Enums.AiRequestStatus.Requested;

    // Dấu Chấm Khởi Động Đồng Hồ Lúc Đầu Nảy Ra Giọt Text AI Trả Về.
    public DateTimeOffset? FirstTokenAt { get; set; }
    // Khép Phiên: AI Kết Loa Tắt Nhịp Lời Chót Ngưng Chảy (End Mark).
    public DateTimeOffset? CompletionMarkerAt { get; set; }
    
    // Lý Do AI Trả Lề Tắt Vội Chặn Tiêu Điểm Bị Kẹt Dài Lời Do Nguồn. (Tự Động Stop, Length Quá Dài ...).
    public string? FinishReason { get; set; }
    // Bị Lag Thử Gọi Lại Được Mấy Lần Quá Hạn Phạt Hoàn.
    public short RetryCount { get; set; }

    // Bản Version Viết Mớm Lời Câu Cài Sẵn Admin Cài Kín AI GPT-4 Để Xét Xử (Prompt Tracking).
    public string? PromptVersion { get; set; }
    // Version Pháp Lý ToS/Điều Khoản AI Đã Ấn Chịu Chơi Lần Này Lôi Ra Cãi Tránh Cãi.
    public string? PolicyVersion { get; set; }
    
    // Mã Tracking Vòng Log Phân Tán Suốt Từ Phía API Cổng Gateway Sang Hang Tận Cùng Server Thấy Nhau Liền 1 Dải (Zipkin/OpenTelemetry Trace & Correlation ID).
    public Guid? CorrelationId { get; set; }
    public string? TraceId { get; set; }

    // Sổ Ghi Nợ Mồm: Trừ Bao Nhiêu Vàng Hay Trừ Nhát Nào Kim Cương (Bảng Giá Lịch Sử Giá Lúc Đầu Bóp Để Tracking Report Kinh Doanh).
    public long ChargeGold { get; set; }
    public long ChargeDiamond { get; set; }

    // Người Đòi Tiếng Gì? Ngôn Ngữ Khách Ép (VI/EN/ZH) Và Ngược Lại Lúc Xuất Trả Tiếng Gì Báo Liền.
    public string? RequestedLocale { get; set; }
    public string? ReturnedLocale { get; set; }
    // Bị Đẩy Về Ngôn Ngữ Dự Phòng Gốc (Ví dụ AI Nó Đen Sì Tiếng Trung Nó Không Biết Chỉ Trả Được Tiếng Anh Vậy Lý Do Tại Cú Là Gì).
    public string? FallbackReason { get; set; }
    
    /// <summary>
    /// Chìa Khóa Vàng Tránh Hoàn Cùng Số Tiền 2 Lần Oan Uổng Nếu Mạng Lắc Gọi Bấm Đi Bấm Lại (Chống Deduplication Nhồi Sóng Idempotency).
    /// Cột Này Mẻ Ném Ra Cục Tên Mã "Rút Hoàn Thất Bại UUIDxyz" - Phóng Cầu Giải Cứu Ghi Sổ Refund SQL 1 Phát Kịch Độc Thôi.
    /// </summary>
    public string? IdempotencyKey { get; set; }
    
    // Ngày Dựng Request:
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    // Ngày Lắc Cái Biến Này Mới Lại. Cần Bám Lấy Giúp Trọng Sổ Cập Cấp.
    public DateTimeOffset? UpdatedAt { get; set; }
    
    // Thuộc tính Navigation Chắp Cánh (Khung Hình Khóa Ngoại Chỉ Tới Đối Tượng Cha Entity User Của Khách Giành Cho Cấu Trúc Relationship Entity Framework Core Cây Tụ Mây EF Core Bao Bọc Dễ Chạy Include<User>()).
    public User User { get; set; } = null!;
}
