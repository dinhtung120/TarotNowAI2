/*
 * ===================================================================
 * FILE: IMfaService.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Phác Thảo Thuật Toán Sinh Sinh Đồng Bộ Token TOTP (Tin nhắn Google Auth).
 *   Sử Dụng cho Admin Hoặc Hệ Thống Đăng Nhập Hai Lớp (MFA - Phase 2.5).
 * ===================================================================
 */

using System.Collections.Generic;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Máy Rang Quay Xổ Số Xác Thực 6 Số (Giống Khi Chọn Bank Transfer Hay Nhập Chuyển Tiền 2FA).
/// Cung Cấp Secret Base32, Sinh Ảnh QR Code, Và Xét Cấp Phép.
/// </summary>
public interface IMfaService
{
    /// <summary>Bốc Ngẫu Nhiên 1 Mớ Mã Hóa Gốc Trắng (Secret) Bằng Chữ Chữ Chữ Cái Xào Base32 Cho Tân Binh Cài Đặt Khóa.</summary>
    string GenerateSecretKey();

    /// <summary>Khóa Rương Chứa Cái Chữ Nhạy Cảm Mới Ép Ra Xuống Két DB Đỡ Bị Hacker Chôm Mất Mã Sinh MFA Của Khách.</summary>
    string EncryptSecret(string plainSecret);

    /// <summary>Mở Chìa Giải Mã Dữ Liệu SQL Về Lại Chữ Trắng Chuẩn Bị Xài.</summary>
    string DecryptSecret(string encryptedSecret);

    /// <summary>Nặn Ra Cái Link Kiểu "otpauth://..." Cấp Sang Javascript Đặng Gọi Thư Viện Vẽ Thành Cục Hình Vuông QR Code Cho Phone Scan Vào Thẳng App.</summary>
    string GenerateQrCodeUri(string plainSecret, string userEmail);

    /// <summary>Cầm Cái Khóa Gốc Tính Ra Xem Liệu 6 Số User Nhập Vào Nó Có Khớp Chu Kỳ Thời Gian 30 Giây Hiện Tại Không Đổi OTP Của Toàn Cầu Không?</summary>
    bool VerifyCode(string plainSecret, string code);

    /// <summary>In Dãy Series Số Dã Chiến Xài Đỡ Nhanh (VD Hư Điện Thoại Mất Google Auth Tính Tính Pass). In Lẹ Cho Nhớ Xài Lần Rớt Lần Nào Là Vứt Luôn.</summary>
    List<string> GenerateBackupCodes(int count = 6);
}
