/*
 * ===================================================================
 * FILE: UserRole.cs
 * NAMESPACE: TarotNow.Domain.Enums
 * ===================================================================
 * MỤC ĐÍCH:
 *   Enum Gọi Tên Các Chức Vụ Quyền Đấng Tôn Của User (Identity Role Đăng Nhập).
 * ===================================================================
 */

namespace TarotNow.Domain.Enums;

/// <summary>
/// Các Quyền Hành Identity Tôn Nghiêm Cho Auth JWT.
/// Quyết Định Mép Khách Tới Chơi Ai, Không Lẫn Hay Rối Nát Cánh Cổng Policy Lớn Tầm Cửa.
/// </summary>
public static class UserRole
{
    // Ngón Ghẻ Chơi Xem Quẹt Gọi Là User Quèn Sớm Ký (Guest Đã Login Tới Check Bói Thường).
    public const string User = "user";
    
    // Khứa Áo Choàng Mọc Cánh Được Cho Nhận Cuốc Kiếm Cục Thẻ Gắn Kiếm Bát Tiền Thật Chạy Chat Nhắn Láy Đọc Bài Sinh Nhai.
    public const string TarotReader = "tarot_reader";
    
    // Lão Cổ Đu Tường Admin Cầm Key Tù Phát Rút Chặn Giam Ban Account Chứ Ko Xem Bói Thui Admin Lương Quản Đốc Ché Vạc Mọi Nhát Rút Withdrawal.
    public const string Admin = "admin";
    
    // Cái Bóng Máng Tự Gọi API Phán Xét Các Webhook Cron Job System Không Là Thằng Human Nào (Tội Chạy Ngu Cho Backend Dùng Background Workers Để Cho Bật Chạy Service Khỏi Role Nhầm Nhột System Quát Không Trừ Sứ Mù).
    public const string System = "system"; 
}
