/*
 * ===================================================================
 * FILE: SpreadType.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Enum Cho Số Thẻ Định Phân Chọn Rút Bói Hình Của Phiên Bói Tùy Mức.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Mẫu Bài Tarot Spread: Tức Là Cục Bài Đó Bạn Khách Sẽ Trải Mấy Lá Lên Bàn Kính. 
/// Gắn Cố Độ Dài Số Mạch Cho App Client Hiển Vẽ Cho Code.
/// </summary>
public static class SpreadType
{
    // Bốc Báo 1 Lá Duy Nhất Thử Vận Free Hoặc Xàm Bói Đầu Ngày.
    public const string Daily1Card = "daily_1";
    
    // Bài Truyền Thống Xòe 3 Lá (Quá Khứ - Hiện Tại - Cầu Tương Lai).
    public const string Spread3Cards = "spread_3";
    
    // Mở Bài Phóng Chữ Số Sinh Học 5 Tấm Gọi Sao Án Liệt (Hút Phí Hơn).
    public const string Spread5Cards = "spread_5";
    
    // Ác Liệt Thủy 10 Lá Gắn Bài Celtic Cross Nhìn Dải Rút Sạc Tiền Mãn Cục Đá Kim Cương (Bảng Đắt Nhất Thường Kéo Thầy).
    public const string Spread10Cards = "spread_10";
}
