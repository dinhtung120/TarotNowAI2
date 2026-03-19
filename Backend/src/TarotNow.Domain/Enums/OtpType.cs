/*
 * ===================================================================
 * FILE: OtpType.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Enum Cho 2 Loại Mã OTP Bắn Cửa Lính Email Xài Lúc Cháy Tài Khoản.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Loại Cột Mốc Lưu Hành Mã 6 Số OTP Nằm Nghẹt SQL.
/// Xác Định OTP Là Do Bọn Nào Yêu Cầu Gì Khắc Thừa Khuyết Lúc Đi Tìm Dò Xóa.
/// </summary>
public static class OtpType
{
    // Thẻ Register Khách Đòi Nhập Mã Vô Email Mới Nảy Trứng. Nâng Gốc App Lên Vừa Đủ Lên Cho Active 4 Cẳng Sạch Acc.
    public const string VerifyEmail = "register";
    
    // Giấy Vang Trắng Quên Đầu Đòi Xin 6 Code Khôi Phục Kêu Điền Lệnh Reset Mật Khẩu Lại.
    public const string ResetPassword = "reset_password";
}
