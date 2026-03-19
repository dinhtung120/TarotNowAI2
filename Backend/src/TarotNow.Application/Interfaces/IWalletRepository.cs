/*
 * ===================================================================
 * FILE: IWalletRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bộ Mặt Ngoại Giao Nghiêm Khắc Nhất Toàn Bộ Hệ Thống: Cổng Ví (Wallet).
 *   Không Cho Ai Sửa Số Tiền Trực Tiếp Chạy SQL (Ví dụ Cấm Update Balance = 5000). 
 *   Phải Buộc Dùng Hàm Credit, Debit Để Database Chạy Trigger Chặn Hack Lỗ Hổng Tiền Âm (Race Condition).
 * ===================================================================
 */

using TarotNow.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Gác Cổng Nhà Băng Bằng Súng. 
/// Giao diện Phân Lớp Ứng Dụng: Gọi Xấp Các Stored Procedure Lưới Lọc Trong SQL "proc_wallet_*" Ép Phía Dưới Chặn Khách Bấm Xoay Tool Hack Tiền Cùng Lúc Khớp 2 Máy 1 Giây Trừ Chồng Chéo.
/// </summary>
public interface IWalletRepository
{
    /// <summary>Nạp Phiếu Trắng Cho Vào Ví Khách (Tăng Tiền/Credit).</summary>
    Task CreditAsync(Guid userId, string currency, string type, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>Rút Sạch Đít Ví Khách Tiêu (Trừ Tiền Mọc Cánh/Debit). Nếu Ko Đủ Báo Lỗi Ngay Ở Database SQL Bắn Ngược Code HỦY Gấp.</summary>
    Task DebitAsync(Guid userId, string currency, string type, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>Thực Hiện Treo Tiền Cho Phiên Trải Bài (Đóng Băng Tạm Thời Làm Escrow Cho Reader Chứ Chưa Chuyển Liền).</summary>
    Task FreezeAsync(Guid userId, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sau Khi Thầy Bói Đọc Hết Chữ AI Xong: Mở Băng Giao Quỹ.
    /// Từ Túi Băng Giá Rơi Trực Tiếp Xuống Túi Của Người Hưởng Thụ Xứng Đáng Tiền VềTúi.
    /// </summary>
    Task ReleaseAsync(Guid payerId, Guid receiverId, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>Thầy Mất Kết Nối Khách Khóc: Hoàn Lại Tiền Từ Túi Đóng Băng Vào Lại Tiền Tươi Túi Cũ Đã Treo Ký (Auto-Refund).</summary>
    Task RefundAsync(Guid userId, long amount, 
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Máy AI Xơi Kim Cương (Mất Vĩnh Viễn Không Bơm Vào Túi Ai Hết - Trừ Túi Treo Cho Bay Xa Tới Provider).
    /// Khác Lệnh Release (Tiền Chạy 2 Túi), Lệnh Khói Này Chỉ Rút Tiền Ở Đống Cục Băng Treo Trên Và Cung Ứng Bày Tỏ Trừ Giam Thành Khói Bụi Không Có Hậu Lưu Viễn Cảnh.
    /// </summary>
    Task ConsumeAsync(Guid userId, long amount,
        string? referenceSource = null, string? referenceId = null, string? description = null,
        string? metadataJson = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);
}
