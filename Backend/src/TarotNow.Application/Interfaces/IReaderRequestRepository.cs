/*
 * ===================================================================
 * FILE: IReaderRequestRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bản Vẽ Giao Thông Tiếp Nhận Đơn Xin Việc. Nơi Giữ Lại Tờ Đơn Khách Thường Nộp Lên Mong Ước Đổi Đời Làm Thầy Bói Chân Chính.
 * ===================================================================
 */

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Thư Viện Chứa Lá Đơn Trực Tuyến Đang Nằm Ở Hàng Chờ Sếp Duyệt (Admin Approval).
/// Thường Nằm Trong Database Mongo Không Gian To Lưu Cấu Trúc Đơn Không Đoán Trước (Flexibility).
/// </summary>
public interface IReaderRequestRepository
{
    /// <summary>User Điền Xong Đơn Dài Thượt Chứa Skill, Lịch Sử Xong Tống Qua Lưu Add Vô Chờ.</summary>
    Task AddAsync(ReaderRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>Admin Thấy Rác Tò Mò Rút Code Id MongoDB Mở Ra Đọc Bảng Xin.</summary>
    Task<ReaderRequestDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thằng Nào Bơm Form Đi Bơm Form Lại Rác DB Tránh Kẹt?
    /// Chặn Ở Cửa Vào Bằng Cách Query Cái Đơn Cũ Nhất Gần Nhất Vẫn Đang "Pending" Thấy Nó Thì Bắt Nó Về Rì Mọt Ngồi Đợi Hạn Không Nộp Đơn Chồng Rác Nữa.
    /// </summary>
    Task<ReaderRequestDto?> GetLatestByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Khuôn Rập Căn Cứ Cho Sếp Trưởng Lướt View App Của Bọn Đòi Nghề Bằng Bảng Lọc: Duyệt Qua Status Đậu Rớt Gì Đọc Lướt Từng Trang (Phân Trang).
    /// </summary>
    Task<(IEnumerable<ReaderRequestDto> Requests, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default);

    /// <summary>Tay Ấn Con Dấu Cộp 'Hợp Lệ Cấp Bằng' Hoặc 'Chê Kém Lùi Về Bồi Dưỡng Lại' Ghi Lịch Sử Lại Bấm Nút Update.</summary>
    Task UpdateAsync(ReaderRequestDto request, CancellationToken cancellationToken = default);
}
