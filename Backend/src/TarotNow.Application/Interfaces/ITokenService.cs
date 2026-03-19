/*
 * ===================================================================
 * FILE: ITokenService.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Biểu Tượng Xí Ngầm Chuyên Nhào Nặn Ra Kìm Khóa Đuôi Json Khép Chặt Bảo Mật Mã Thông Báo Đeo Cổ Dành Cho Khách Hàng (JSON Web Token - JWT).
 *   Và Cục Phóng Mã Random Dài Tươi Nặn Quà Ra RefreshToken.
 * ===================================================================
 */

using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Thầy Ấn Tín Pháp Lưu Thông Mộc Dấu Đẩy Khách Trôi Tự Do Giữa Vô Hình Dải API Đọc Tin JWT Dịch Khắp Rừng.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Gom Góp Thu Cục Tên, Giới Tính Khách Trộn Lại Ra Thỏi Kem Khắc Chấm JWT Mẫu Truy Cập Dừng Đúng Hạn 30 Phút Cháy Rụp.
    /// Cái Này Gọi Lên Access Token Phải Viết Mật Ký Chống Giả Phá Gieo Bọn Mạo Danh Nắm Thể Thức (Cổng Token Bốc Cháy Code Ở Dưới Infrastructure JWT Lib).
    /// </summary>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Quăng 1 Cục Bọc Base64 Dày Đặc Kích Khủng (Lớn Thiệt Bằng 32-64 Bytes) Xoắn Cục Cho Bất Kỳ Mạo Phấn Tìm Dấu Bẻ Khóa Nào. 
    /// Dành Cất Riêng Hút Pass Kéo Tuổi Thọ Access Cũ (Refresh Token 7 Ngày).
    /// </summary>
    string GenerateRefreshToken();
}
