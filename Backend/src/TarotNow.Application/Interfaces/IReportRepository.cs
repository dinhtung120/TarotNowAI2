/*
 * ===================================================================
 * FILE: IReportRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bộ Mặt Ngoại Giao Quản Lý Danh Sách Sự Trách Móc (Reports).
 *   Các Thông Điệp Ăn Vạ Này Thường Được Thải Sang Cửa MongoDB Để Tự Do Miêu Tả Cũ Mới Chữ Chữ Chửi Đổng Dài Dài Tùy Hỷ Khách Hàng.
 * ===================================================================
 */

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Hòm Cự Cãi Tố Cáo Đứa Nào Đó Spam Nhảm Nàng Tiên Lời Dối Trán Hoặc Admin Giàn Xếp Tranh Chấp Trút Bực Dọc Khi Reader Thu Tiền Không Xem Bàỉ.
/// </summary>
public interface IReportRepository
{
    /// <summary>Viết Lờ Tranh Cãi Nhắm Chị Mất Gà Tố Trễ Bỏ Thư Vào Hộp.</summary>
    Task AddAsync(ReportDto report, CancellationToken cancellationToken = default);

    /// <summary>
    /// Admin Bê Hòm Rác Tố Biểu Kéo Từng Đống Thư Ra Xem Chuyển Vào Nhóm Nào Đã Phán Xử Giải Tiền,
    /// Bọc Nhóm Mới Chữ Chữ (Lọc Qua Bộ Phân Trang Paginate Bớt Ngộp Chữ).
    /// </summary>
    Task<(IEnumerable<ReportDto> Items, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? statusFilter = null,
        CancellationToken cancellationToken = default);
}
