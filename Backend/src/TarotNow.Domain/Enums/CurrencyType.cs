/*
 * ===================================================================
 * FILE: CurrencyType.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bộ Enum Cho Lõ Lại Tiền Tệ Định Mệnh Củ App Chỉ 2 Loại.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Các Thuật Ngữ Định Danh Đơn Vị Giọt Khối Lượng Tiền Tệ Dùng App.
/// Giảm Lỗi Gõ Sai Chữ Lúc Code Database: "gold", "diamond".
/// </summary>
public static class CurrencyType
{
    // Đồng Tiền Nhặt Của Rơi Rơi 
    public const string Gold = "gold";
    
    // Đá Lửa Máu Đỏ Kim Nạp Tiền Túi Rút
    public const string Diamond = "diamond";
}
