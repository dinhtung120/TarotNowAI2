/*
 * ===================================================================
 * FILE: ChatQuestionItem.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Table: Bản Mệnh Từng Dòng Ngửa Bàn Trả Gía Của Từng Câu Hỏi Khi Thuê Reader (ChatQuestionItem).
 *   Kèm Đặt Đồng Hồ Bom Hẹn Giờ (24h Lệnh Đuổi Nếu Thầy Im Re Sẽ Bật Rả Tiền Khách).
 * ===================================================================
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Mẫu Trạm Phạt (Entity) Nằm Trong Vạch Giam (ChatFinanceSession). 
/// Đây Cục Giam Tiền Kích Thước Tái Lấp Từng Chấm Một: Ví Dụ Khách Hỏi (Trừ Cọc 2 Kim Cương Vô Đây Lập Biên Bản).
/// Bom Đặt 24 Giờ Nhát Đứt Phân Giải Tự Auto Bật Ra Logic Escrow Cho Máy Background Jobs Đo Hạn (Treo Treo Tiền Cụ Thể Từng Lần Phóng Lời Question Của Khách).
/// </summary>
[Table("chat_question_items")]
public class ChatQuestionItem
{
    // Bút Tích Mã Lượt Giam Tiền Tách Biệt Mỗi Cục Hẹn Giờ Riêng Lẻ.
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    // Lồng Trọng Lệnh Dính Trỏ Vào Bọc Phiếu Bố Ở Table Ngoài (FinanceSession).
    [Column("finance_session_id")]
    public Guid FinanceSessionId { get; set; }

    /// <summary>Mã Cầu Cấm MongoDB Cuốn Tạp Chí Chat Đứng Chồng Dĩ.</summary>
    [Column("conversation_ref")]
    [Required]
    public string ConversationRef { get; set; } = string.Empty;

    /// <summary>Kẻ Xì Tiền Đưa Cọc Cho Lời Chúc (User).</summary>
    [Column("payer_id")]
    public Guid PayerId { get; set; }

    /// <summary>Quan Nhân Đi Ăn Hoa Hồng Cất Lộc Nào (Reader).</summary>
    [Column("receiver_id")]
    public Guid ReceiverId { get; set; }

    /// <summary>Tiêu Loại Mấu Câu Gì (Câu Gốc Có Gói Rẻ Hơn Hay Câu Follow_Up Chặt Chém Kiều Giá Độn Khách Đâu: main_question / add_question).</summary>
    [Column("type")]
    [Required]
    public string Type { get; set; } = "main_question";

    /// <summary>Cục Thạch Trân Châu Lấp Lánh Chính Thức Tảng Đóng Bằng Cột Chặn Của Phép Treo Từng Câu (Số Tiền Diamond Của Tần Này Ngậm Lại Chờ Đoán Mệnh Xong Ai Ăn Tiền Sẽ Đưa Đi Xuống DB).</summary>
    [Column("amount_diamond")]
    public long AmountDiamond { get; set; }

    /// <summary>Giao Tuyến Sinh Động Của Trái Cầu Vồng Ghi Sổ Đi Bói: Khởi Đầu pending (Đợi Gật Đầu), accepted (Thầy Bắt Lời Chịu Giải), released (Tháo Băng Bỏ Túi Thầy), refunded (Bật Ngược Hoàn Khách 100%), disputed (Bao Nháo Kiện Tung Nẩy Tòa Án Admin Dòm Ngó).</summary>
    [Column("status")]
    [Required]
    public string Status { get; set; } = "pending";

    /// <summary>Chữ Khách Chào Bắn Tin MongoDB Câu Hỏi Nội Dung Nhét Sàng Chữ Nằm (Mã Dài MongDB Document Cũ Thất). Thẻ Này Lưu Trữ Bồi Câu Tên Gì ID Message Bên Kia.</summary>
    [Column("proposal_message_ref")]
    public string? ProposalMessageRef { get; set; }

    /// <summary>Khung Bom Treo Thử 1 Thầy Này Vứt Đó Cả Tháng Có Sao - Thì Có Kẹp Bom Nếu Tới Lúc Máy Đi Ngang Thấy Đít Nó Nóng Quá Hạn Là Refund Vã Vì Reader Có Xác Kêu Trả Đéo Thèm Nhận Lời Gì Trong Suốt (Expires Limit Thời Giao Quẹo).</summary>
    [Column("offer_expires_at")]
    public DateTime? OfferExpiresAt { get; set; }

    // Dấu Ấn Thầy Bảo Ơi Vâng Con Giải Cứ Cọc Chắc Đồng Ý Lúc Chấm X.
    [Column("accepted_at")]
    public DateTime? AcceptedAt { get; set; }

    /// <summary>Chiếc Kim Bơm (Thời Gian Chịu Accept Lại Cộng Trái Chuối 24h Đều Giữ 1 Ngày Buộc Phải Gõ Bài Đem Trả, Không Kẹp Thời Khắp Không Lấy Tiền Kéo Cuống Thời Hạn Nhìn Reader Trả Lời Trễ).</summary>
    [Column("reader_response_due_at")]
    public DateTime? ReaderResponseDueAt { get; set; }

    // Khoảnh Khắc Nóng Ấm Bàn Tay Thầy Đóng Xong Lệnh Chú Chú Dấu Nhốt Đi Trả Hoàn Thành Xong (Cái Phím Lái Mở Dọng Trả Lời Mấu Reader).</summary>
    [Column("replied_at")]
    public DateTime? RepliedAt { get; set; }

    /// <summary>Rót Lời Xong Chưa Thưởng Liền (Giam Cho Đi Chợ Quả Bực Ốp 24H Kiện). Đồng Hồ Kiểu Chuyển Tiền Cho Túi Của Lão 24 Tiếng Nữa Đếm Giờ Nổ Máy Hồi Bơm Tức Giao Túi Tiền Cho Reader (Cho Tự Hốt Do Khách Đéo Mở Cố Xem Khủng Bỏ Phí Khách Mặc Chạy Lố Án Free To Reader: Auto-Release).</summary>
    [Column("auto_release_at")]
    public DateTime? AutoReleaseAt { get; set; }

    /// <summary>Bơm Oan Nếu Không Chịu Trả Lời Thầy Hứa Gà Vã 24 Tiếng Không Rằng Rep Cho Nên Trả Đi Ngược Về Cho User (Auto-Refund: Không Tính Tiền Bán Thầy Bói Ma).</summary>
    [Column("auto_refund_at")]
    public DateTime? AutoRefundAt { get; set; }

    // Dấu Phẩy Xả Tiền.
    [Column("released_at")]
    public DateTime? ReleasedAt { get; set; }

    // Khách Xóa Trầm Luân Không Phải Ép Đợi Chọn Tốt Kéo Phóng Tiền Mặc Kệ 24h Quá Sớm Cho Nhanh.
    [Column("confirmed_at")]
    public DateTime? ConfirmedAt { get; set; }

    // Túi Quay Hồi Bích Dấu Rửa Tiền Ác Dành Refund.
    [Column("refunded_at")]
    public DateTime? RefundedAt { get; set; }

    /// <summary>Lưới Vét Khung Kiện Tranh Cưởi Tranh Cắn Căn Phòng Án Ánh Đỏ Có 24h Còi Kêu Ọ Ọ (Giờ Giải Quyết Khách Kiện Reader Report Xin Hoàn Oan Trái Cấm Xâm Chiếm Ví Trống Mất).</summary>
    [Column("dispute_window_start")]
    public DateTime? DisputeWindowStart { get; set; }
    [Column("dispute_window_end")]
    public DateTime? DisputeWindowEnd { get; set; }

    /// <summary>Mã Định Danh Giết Đúp Chạy Chống Rơi (Idempotency Lõi Gấp Ác Liệt DB Giải Tiền An Khang Xóa).</summary>
    [Column("idempotency_key")]
    public string? IdempotencyKey { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Mối Liên Giao SQL Khía Trở Về Với Lệnh Phiếu Giam Cha (Mục Navigation EF Core Dùng Đọc Ráp SQL Ngoại).
    [ForeignKey("FinanceSessionId")]
    public virtual ChatFinanceSession? FinanceSession { get; set; }
}
