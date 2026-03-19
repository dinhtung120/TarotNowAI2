/*
 * ===================================================================
 * FILE: IDepositPromotionRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Diện Bản Vẽ Chứa Kho Chứa Các Chương Trình Khuyến Mãi (Sale Off / Nạp Tích Lũy Bốc Quà).
 *   Giúp Marketing thao túng giá trị nạp của người dùng.
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Kho Báu Lưu Chứa Thể Thức Xóc Đĩa Sale Đầu Năm Hoặc Event Tặng Nạp.
/// Dùng để cho hệ thống đọc ra rồi ép vào Đơn Hàng tính chiết khấu hoặc Thêm Kim Cương Phụ Phí.
/// </summary>
public interface IDepositPromotionRepository
{
    /// <summary>Móc Một Gói KM Bằng ID Bốc Thăm (Guid).</summary>
    Task<DepositPromotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Danh Sách Thể Thức (Admin Xem Toàn Bộ Rác / Đóng / Mở Để Quản Lý).</summary>
    Task<IEnumerable<DepositPromotion>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Chỉ móc ra những chương trình Sale nào ĐANG DIỄN RA Trưng Bày Cho Khách Xem Kích Nạp.</summary>
    Task<IEnumerable<DepositPromotion>> GetActivePromotionsAsync(CancellationToken cancellationToken = default);

    /// <summary>Ký Tạo Mùa Khuyến Mãi Mới Dành Cho Marketing (New Year Sale 50%).</summary>
    Task AddAsync(DepositPromotion promotion, CancellationToken cancellationToken = default);

    /// <summary>Điều Chỉnh Lại Hạn Chót Hoặc Lợi Nhuận Gói Sale (Update).</summary>
    Task UpdateAsync(DepositPromotion promotion, CancellationToken cancellationToken = default);

    /// <summary>Hủy Sạch Thể Thức Lỗi (Delete Gói Thay Vì Tắt Khóa).</summary>
    Task DeleteAsync(DepositPromotion promotion, CancellationToken cancellationToken = default);
}
