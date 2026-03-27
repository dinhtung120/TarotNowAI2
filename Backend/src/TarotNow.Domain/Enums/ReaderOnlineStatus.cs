/*
 * ===================================================================
 * FILE: ReaderOnlineStatus.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Trạng Thái Biển Treo Cửa Của Thầy Bói (Có Khách Vô Hay Tắt Đèn Bấm Nút Đi Ngủ Kệ Chat Mời).
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Trạng thái biển chỉ dẫn trực tuyến của Reader để bọn UI ngoài sảnh Search List Hiển Thị Nhấp Nháy Hàng Mời Khách Đi Ngang.
///
/// Tại sao Constant Chữ Ở SQL Mongo?
/// → Nhất quán với Các Pattern Status khác (UserStatus, ReaderApprovalStatus).
/// → Mắt người đọc file JSON Database dễ hơn đọc mớ Enum số 0 1 2 lủm chủm đi debug rát đầu.
///
/// Đặc biệt dính luật P2-READER-QA-1.2: Cổng Gate Kiểm Duyệt Phải Nhất Thiết Qua Chữ "accepting_questions" Mới Cho Nhắn Text.
/// </summary>
public static class ReaderOnlineStatus
{
    /// <summary>Có Mặt Ở Trên Web Nhưng Treo Máy Không Ghi Cửa Đèn Đỏ Vẫn Ẩn (Không Thẻ Chat Nhắn Qua Chặn).</summary>
    public const string Online = "online";

    /// <summary>Giật Cục Rút Dây Điện Hoặc Nhấn Nút Tắt Ca Nghỉ Ẩn Vào Góc Không Thấy Mặt Ở Card List Directory Trang Chủ.</summary>
    public const string Offline = "offline";

    /// <summary>Sẵn Sàng Mở Lò Bát Quái Đón Tiền Đón Thớt Lên (Trạng Thái Xăng Dầu Độc Nhất Ưu Tiên Mở Barrier Check Ném Chat Yêu Cầu Kết Nối Bằng Kim Cương User).</summary>
    public const string AcceptingQuestions = "accepting_questions";

    /// <summary>Reader tạm rời máy nhưng chưa tắt ca hoàn toàn; vẫn cho phép mở chat nhưng cần cảnh báo SLA có thể chậm.</summary>
    public const string Away = "away";

    public static bool IsValid(string? status)
        => TryNormalize(status, out _);

    public static bool TryNormalize(string? status, out string normalized)
    {
        normalized = Offline;
        if (string.IsNullOrWhiteSpace(status))
        {
            return false;
        }

        var value = status.Trim().ToLowerInvariant();
        switch (value)
        {
            case Online:
            case "active":
            case "connected":
            case "available":
                normalized = Online;
                return true;
            case Offline:
            case "disconnected":
            case "invisible":
                normalized = Offline;
                return true;
            case AcceptingQuestions:
            case "acceptingquestions":
            case "accepting-questions":
            case "accepting":
            case "ready":
                normalized = AcceptingQuestions;
                return true;
            case Away:
            case "idle":
                normalized = Away;
                return true;
            default:
                return false;
        }
    }

    public static string NormalizeOrDefault(string? status, string defaultStatus = Offline)
        => TryNormalize(status, out var normalized) ? normalized : defaultStatus;
}
