/*
 * ===================================================================
 * FILE: IWithdrawalRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Theo Dõi Giấy Rút Gạo Phiếu Ra ATM: Giao Diện Withdrawal Request.
 *   Xử Lý Vụ Khách Xin Trút Kim Cương Ghi Thành Lệnh SQL SQL Chờ Sếp Ký Quyết Định Duyệt Nhả Tích Băng Tiền.
 * ===================================================================
 */

using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Thùng Chứa Hồ Sơ Đòi Tiền (SQL).
/// Lưu ý Rằng Hàm Này Không Cắn Vào Trừ Ví. Việc Cắn Ví Phải Là Việc Phối Phối Ở Cái Interface Của Máy Chém Wallet Kia Trực Tiếp Nghen.
/// </summary>
public interface IWithdrawalRepository
{
    /// <summary>Đào Mẫu Thẻ Đòi Rút Ra Đối Chiếu Thông Tin Lệnh.</summary>
    Task<WithdrawalRequest?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Cấm Nện Cái Link Đòi Tiền Quá Nhiều Lần Trong Hôm Lẽ Kèo Gây Nghẽn (Anti-Spam Cho Admin Phù).</summary>
    Task<bool> HasPendingRequestTodayAsync(Guid userId, DateOnly businessDate, CancellationToken ct = default);

    /// <summary>Danh Sách Thầy Bói Coi Của Tui Tháng Nào Chờ Tháng Nào Bị Rớt Đọc Được Tổng Phiên.</summary>
    Task<List<WithdrawalRequest>> ListByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);

    /// <summary>Góc Nhìn Chúa Trời: Admin Thấy Đứa Nào Đang Khóc Mong Tiền Trôi Rì Rầm Hàng Loạt.</summary>
    Task<List<WithdrawalRequest>> ListPendingAsync(int page, int pageSize, CancellationToken ct = default);

    /// <summary>Ném Tờ Mẫu Xong Bơm Xin Thất Kí Gửi.</summary>
    Task AddAsync(WithdrawalRequest request, CancellationToken ct = default);

    /// <summary>Ký Duyệt "Tao Đã Bank Nhé" Ghi Cái Ảnh Nhấp Nhô Trạng Thái DONE.</summary>
    Task UpdateAsync(WithdrawalRequest request, CancellationToken ct = default);

    /// <summary>Unit Of Work: Kết Bài Đặt Bút Xuống Chốt SQL.</summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
