/*
 * ===================================================================
 * FILE: ChatFinanceSession.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Entity Table: Ánh Xạ Bảng Cơ Sở SQL Quản Lý Quỹ Treo Thưởng Kì Kèo Tạp Chí (Escrow).
 *   Mỗi Cuộc Gọi Nhắn Tin Tán Ngẫu Bằng Chat Thue Với Reader Sẽ Sinh Ở Đây Sinh Tiền Cọc Chung Bọc Khoảng Lại Hẹn Hò Giữ Tiền An Toàn.
 * ===================================================================
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Mẫu Trạm Đóng Gói (Entity) Châm Bản Ghi Khung Bảng SQL Của Bọn Kế Toán Giam Tiền Hai Cửa (Escrow).
/// Chuyên Lo Chứa Tổng Bọc Quỹ To Của Toàn Cả Đợt Nhắn Chat Dài Hôm Qua Tới Nay (Và Nối Đi Các Nhát Giam Tiền ChatQuestion Kèm Theo Của Người Đẹp).
/// 1 Conversation Ở Mongo (Cửa Thoại Chat) Kẻ Vạch Đối 1 FinanceSession Này (Bề Mặt Bọc Tiền).
/// </summary>
[Table("chat_finance_sessions")]
public class ChatFinanceSession
{
    // Cột Chìa Khóa Trụ Cột Mã Dịch PK Để Phân Biệt Từng Khung Treo Tiền Khóa Lại.
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Mã Mongo Xuyên Không Gian Gí Sang 2 Khu Database Khác Nhau Dùng Mối Ráp Này Gắn Cuốn Ghi Chú Chat Text Bên Mongo (Conversation_Id dài 24 Hex Char).
    /// </summary>
    [Column("conversation_ref")]
    [Required]
    public string ConversationRef { get; set; } = string.Empty;

    /// <summary>Túi Tiền Bên Thua - Khách Này Là Người Rút Kim Cương Gửi Quỹ Ủy Thách.</summary>
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>Ngồi Quầy Ké Thu Thưởng Phạt Nhận - Thầy Reader (Cũng Là Guid Trỏ Về Base Của Bảng User Đăng Nhập).</summary>
    [Column("reader_id")]
    public Guid ReaderId { get; set; }

    /// <summary>
    /// Vành Đai Trạng Thái Chung Phiếu Đóng Gói Lớn Khóa Đầu: Đang Treo Câu (pending), Rụng Túi (active), Rút Xong Tất Cả (completed), Trả Về Khách (refunded)...
    /// </summary>
    [Column("status")]
    [Required]
    public string Status { get; set; } = "pending";

    /// <summary>Tổng Hợp Lượng Vàng/Kim Cương Mà Thằng Session Này Hiện Bóp Cổ Treo Bao Nhiêu Sẽ Cấm Thằng Khách Xài Trong Ví Ở Ngoài Đi Mua Rác Khác Bị Hết (Freeze Count).</summary>
    [Column("total_frozen")]
    public long TotalFrozen { get; set; }

    // Khoảnh Khắc Lập Giao Thước Hợp Đồng Dấu Time SQL Bắt Mức Khuy.
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Biên Cương Cập Nhật Dấu In Khi Rút Từng Thỏi Đá Nào Về Đâu Thì Xê Chỉnh Update Date Này Chớp Lên.
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Móc Nối Bàn Tay Liên Đới Tới Bọn Con Lũ Tiểu Tốt (Các Câu Hỏi Nhỏ Đã Bóp Cọc Tiền) (Khung Quan Hệ DB Con Kẹp EF Core).
    public virtual ICollection<ChatQuestionItem> QuestionItems { get; set; } = new List<ChatQuestionItem>();
}
