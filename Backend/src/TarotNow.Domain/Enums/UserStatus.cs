/*
 * ===================================================================
 * FILE: UserStatus.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Enum Trang Thái Của Bản Thân Cái Xác Domain User Entity Bên Trong.
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Biển Cấm Lộ Bày Cửa Tài Khoản Trạm Trú Phase (Đã Bị Giết, Hay Chưa Duyệt Cổ Tích).
/// Quy Tắc Trạng Thái Báo Ám DB Lồng Trong User Table:
/// </summary>
public static class UserStatus
{
    // Khách Vừa Ngáp Vô Form Đăng Ký Xong Phun Tới Nhận Mail Chờ Code Chưa Điền Được Vô Phải Pending Đứng Im (Vẫn Login Bị Chặn Ra Mã).
    public const string Pending = "pending";
    
    // Nộp Code Lành Đã Chữa Tích Xanh Tick Mở Auth Active Ròng JWT Chảy Cùng Nước Lấy Tiền Làm Trải Bước Gọi Các Chỗ API Được Khơi Chạy Đời Thoải Mái Ngủ Bằng.
    public const string Active = "active";
    
    // Ác Án Khóa Đèn Chứa Dân Treo Trầm Kệ Thằng Lỗi Mật Khẩu Khóa Kẹt Đập Tạm 30 Phút Sợ Hacker Lọt Tạm Đứng Chế Locked Chờ Mail.
    public const string Locked = "locked";
    
    // Lụi Xuýt Mõm Trù Ép Die Tủ Cho Giam Vào Nhà Tù Tử Admin Ném Sọt Rác Chống Hồi Sinh Do Vi Phạm Nghiêm Trọng TOS Chửi Tục Hack Tiền Banned Không Cho Đẻ Gọi Vào Dòng Giao Diện Nữa.
    public const string Banned = "banned";
}
