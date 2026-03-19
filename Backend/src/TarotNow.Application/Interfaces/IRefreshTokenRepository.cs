/*
 * ===================================================================
 * FILE: IRefreshTokenRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Diện Bản Vẽ Sổ Sứ Mệnh Chìa Khóa (Refresh Token).
 *   Bảo Tri Lưu Dấu Các Thẻ Refresh Token Lên Xuống Cơ Sở Dữ Liệu SQL Nhằm Đòi Lại Đồ Đã Cấp Nếu Bị Lộ Pass.
 * ===================================================================
 */

using TarotNow.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Kho Giam Giữ Tấm Vé Máy Bay Hạng Thương Gia (Refresh Token).
/// Vé Này Cầm Ra Có Thể Xin Được Khóa Ngắn Hạn Mới (Access Token).
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>Bốc Mảnh Vé Dựa Vào Dòng Chữ Dài Trên Cực Khó Nhớ Lấy Chữ Chữ Hàng Trăm Ký Tự Xác Minh.</summary>
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>Cất Giấu Cuộn Vé Nhét Nó Xuống Kho Cung Cấp Thông Điệp Để Đối Chiếu Cho Tương Lai Khách Xin Mới Pass.</summary>
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    /// <summary>Lấy Giao Bút Viết Gạch Bỏ Tờ Giấy Sổ Trắng Xóa Cái Hiệu Lực Hoặc Chuyển Phiếu Khác Hủy Chứ Không Xóa Trắng Mất Vết.</summary>
    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Thắt Cổ Gộp Tất Cả Các Máy Đang Đăng Nhập Điện Thoại Cảnh Báo "Đít Sờ Lốc" Ra Trái Trái Cửa Đuổi Hết Bọn Gian Tính Của Mình Giả Mạo.
    /// Tính Chất Thu Thập Toàn Bộ Token Tống Cổ Do Khách Báo Mất Máy IP.
    /// </summary>
    Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
