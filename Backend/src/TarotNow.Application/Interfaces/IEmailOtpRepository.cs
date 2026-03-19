/*
 * ===================================================================
 * FILE: IEmailOtpRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Diện Nằm Vùng Dịch Vụ Lưu Giữ Mã OTP (One-Time-Password).
 *   Sử dụng Để Đảm Bảo Mã Đi Qua Chặn An Toàn Xác Minh Người Nhận Của Tín Hiệu (Đăng ký/Quên Mật Khẩu/MFA).
 * ===================================================================
 */

using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Giao Diện Cho Kho Ngăn Kéo Lưu Đựng Mã Số Bí Ẩn (OTP).
/// Sổ sách ghi lại cái mã này được gửi cho ai, số Mấy? Khi Nào Hết Hạn? Đã Xài Chưa?
/// </summary>
public interface IEmailOtpRepository
{
    /// <summary>Luồn Mã Đã Tự Sinh Dành Dụm Vào Phểu Cất Giữ (Sẽ gửi lên Mail Đồng Bô Đi).</summary>
    Task AddAsync(EmailOtp otp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tìm Kiếm Mã Cuối Cùng Còn Còn Hạn Sử Dụng Nào Đó Xem Có Trùng Kiểu (Đăng Ký, Đổi Mật Khẩu, Lấy Đồ...) Của Người Dùng Đang Thử Vào.
    /// Nếu Hết Hạn Nhỏ Giọt Phải Xin Mã Mới Qua Mail.
    /// </summary>
    Task<EmailOtp?> GetLatestActiveOtpAsync(Guid userId, string type, CancellationToken cancellationToken = default);

    /// <summary>Kéo Chuông Bất Hoạt Khóa Nhốt OTP Phập Lại: Thằng Nào Đã Dùng Pass Rồi Thì Tắt Lửa Đi Đừng Cho Xài Nữa Mất An Toàn.</summary>
    Task UpdateAsync(EmailOtp otp, CancellationToken cancellationToken = default);
}
