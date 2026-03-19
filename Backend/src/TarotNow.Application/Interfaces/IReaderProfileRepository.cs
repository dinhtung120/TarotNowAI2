/*
 * ===================================================================
 * FILE: IReaderProfileRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Diện Bản Vẽ Sổ Tay Hồ Sơ Lăng Xê Cho Các Thầy Bói (Reader: Những Chuyên Gia Xem Tarot Đã Approve).
 *   Giữ Kho Dữ Liệu Ở MongoDB Và Application Chỉ Kéo DTO Ra Vô.
 * ===================================================================
 */

using TarotNow.Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Tổ Chức Profile Công Khai Của Reader Dành Cho Khách Hàng Book Lịch Hoặc Xem Info.
/// Đảm bảo Application không Lấm Lem Bùn Đất "Document" Của MongoDB Gốc.
/// </summary>
public interface IReaderProfileRepository
{
    /// <summary>Nhập Sổ Thành Môn Chuyên Nghiệp: Sau Khi Đơn Xin Admin Đậu, Tạo Sổ Profiler Công Tác (Tạo Reader Mới).</summary>
    Task AddAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default);

    /// <summary>Tra Giở Hồ Sơ Bằng Id Chứng Minh Thằng User (Postgres Auth -> Lấy Kèm Data Reader Ở Mongo).</summary>
    Task<ReaderProfileDto?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>Thầy Mới Nâng Phí Cắt Cổ, Đổi Quẻ Phép, Thay Bio Update Cho Vào Đóng Dấu Cái Cụp Lại.</summary>
    Task UpdateAsync(ReaderProfileDto profile, CancellationToken cancellationToken = default);

    /// <summary>Rạch Mặt Kéo Tên Xuống Hầm (Rút Thẻ Hoành Nghề). Ẩn Mềm Không Ai Thấy Nữa Nhưng Vẫn Lưu Lịch Sử Admin Trị Tội (Soft Delete).</summary>
    Task DeleteByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bộ Loc Trải Bài Quảng Cáo Ra Cho Khách Lướt (Reader Directory).
    /// Kèm Theo Tính Năng Có Search Từ Khoá Và Lọc Chỉ Mấy Ông Lên Online Sống Động Mà Phân Băng (Paginate).
    /// </summary>
    Task<(IEnumerable<ReaderProfileDto> Profiles, long TotalCount)> GetPaginatedAsync(
        int page, int pageSize,
        string? specialty = null, string? status = null, string? searchTerm = null,
        CancellationToken cancellationToken = default);
}
