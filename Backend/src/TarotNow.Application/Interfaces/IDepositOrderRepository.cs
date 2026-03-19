/*
 * ===================================================================
 * FILE: IDepositOrderRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bản Vẽ Giao Diện (Interface) với hệ thống Cơ Sở Dữ Liệu Lưu Trữ Biên Lai Nạp Tiền (Deposit).
 *   Tại Đây quản lý các Đơn Hàng Đang Chờ Duyệt (Pending) hoặc Đã Cập Nhật Trạng Thái (Thành Công/Hủy).
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Trích Lục Hồ Sơ Thanh Toán Nạp Tiền. Giao kèo để lớp ứng dụng (Application) không cần biết 
/// Data nằm ở SQL, Oracle hay File Text.
/// </summary>
public interface IDepositOrderRepository
{
    /// <summary>Tra Giấy Biên Nhận Bằng Mã Nhận Dạng (ID).</summary>
    Task<DepositOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kẹp Chặt Kẹp Giấy Biên Nhận (For Update Lock). Dùng khi đang Cập Nhật Tiền Tránh Trùng Lặp 2 Luồng Gõ Vào Cùng 1 Bill (Double Tapping).
    /// </summary>
    Task<DepositOrder?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Đào Lên Phiếu Thu Dựa Theo Mã Bên Cổng Thanh Toán Gửi (VD: VNPAY_123456).</summary>
    Task<DepositOrder?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thu Gom Lại Toàn Bộ Những Thằng Chốt Kèo Mà Chưa Chuyển Tiền 
    /// Đã Quá Hạn Bao Lâu Rồi Dọn Hết Để Giải Phóng Dữ Liệu Rác (Cronjob Auto-Cancel).
    /// </summary>
    Task<IEnumerable<DepositOrder>> GetPendingOrdersAsync(TimeSpan olderThan, CancellationToken cancellationToken = default);

    /// <summary>Bưng 1 Khay Biên Lai Cho Admin Ngâm Cứu Theo Phân Trang. Thích thì lọc theo Tình Trạng (Đang Chờ/Đã Duyệt).</summary>
    Task<(IEnumerable<DepositOrder> Orders, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? status, CancellationToken cancellationToken = default);

    /// <summary>Vứt Đơn Nạp Tiền Mới Tinh Vào Giỏ Cho Nhân Viên Check.</summary>
    Task AddAsync(DepositOrder order, CancellationToken cancellationToken = default);

    /// <summary>Gạch Dấu Duyệt Xong Xuống Cuốn Sổ (Xác nhận/Hủy Bỏ Giao Dịch Nạp).</summary>
    Task UpdateAsync(DepositOrder order, CancellationToken cancellationToken = default);
}
