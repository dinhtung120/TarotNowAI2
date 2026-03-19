/*
 * ===================================================================
 * FILE: EconomyConstants.cs
 * NAMESPACE: TarotNow.Domain.Constants
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa Các Hằng Số Bất Biến Về Tiền Tệ Và Các Luật Lệ Tỉ Giá Rắn.
 *   (Centralized Nơi Cấu Hình Hard-Coded Các Limit Hoặc Quy Đổi Tỉ Lệ Đóng Bục Tránh Sửa Chồng Chéo Code Logic).
 * ===================================================================
 */

namespace TarotNow.Domain.Constants;

/// <summary>
/// Các Mốc Cố Định Của Nền Kinh Tế Vàng Và Kim Cương Tarot Now.
/// </summary>
public static class EconomyConstants
{
    /// <summary>
    /// Tỷ giá Cố định: 1000 Đồng (VND) đổi được 1 Kim Cương (Diamond).
    /// Đây là Base Mốc Tính Toán Đổi Tiền Khi Khách Nạp VNPay Số Lượng Nhiêu Thì Sang Diamond Tương Ứng.
    /// </summary>
    public const long VndPerDiamond = 1000;
}
