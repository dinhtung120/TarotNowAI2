/*
 * FILE: DepositPromotionRepository.cs
 * MỤC ĐÍCH: Repository quản lý bảng deposit_promotions (PostgreSQL).
 *   Khuyến mãi nạp tiền: "Nạp 100k VNĐ → tặng thêm 50 Diamond bonus!"
 *
 *   CÁC CHỨC NĂNG:
 *   → GetByIdAsync: tìm khuyến mãi theo ID
 *   → GetAllAsync: lấy tất cả (Admin quản lý)
 *   → GetActivePromotionsAsync: lấy chỉ khuyến mãi đang bật (hiển thị cho User)
 *   → Add/Update/Delete: CRUD cho Admin
 */

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implement IDepositPromotionRepository — truy cập bảng deposit_promotions.
/// </summary>
public class DepositPromotionRepository : IDepositPromotionRepository
{
    private readonly ApplicationDbContext _context;

    public DepositPromotionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>Tìm khuyến mãi theo ID.</summary>
    public async Task<DepositPromotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DepositPromotions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Lấy TẤT CẢ khuyến mãi (bật + tắt) — dùng cho Admin dashboard.
    /// Sắp xếp theo MinAmountVnd giảm dần (mức nạp lớn nhất hiện trước).
    /// </summary>
    public async Task<IEnumerable<DepositPromotion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DepositPromotions
            .OrderByDescending(p => p.MinAmountVnd)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Lấy CHỈ khuyến mãi đang bật (IsActive = true) — hiển thị cho User trên trang nạp tiền.
    /// Sắp xếp giảm dần theo MinAmountVnd để:
    ///   - Hiện mức nạp lớn nhất trước (khuyến khích User nạp nhiều)
    ///   - Hệ thống tìm khuyến mãi phù hợp = lấy mức đầu tiên có MinAmountVnd ≤ số tiền nạp
    /// </summary>
    public async Task<IEnumerable<DepositPromotion>> GetActivePromotionsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DepositPromotions
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.MinAmountVnd)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Thêm khuyến mãi mới (Admin tạo).</summary>
    public async Task AddAsync(DepositPromotion promotion, CancellationToken cancellationToken = default)
    {
        await _context.DepositPromotions.AddAsync(promotion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Cập nhật khuyến mãi (ví dụ: Admin bật/tắt, sửa mức thưởng).</summary>
    public async Task UpdateAsync(DepositPromotion promotion, CancellationToken cancellationToken = default)
    {
        _context.DepositPromotions.Update(promotion);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Xóa CỨNG khuyến mãi khỏi DB (hard delete).
    /// Lưu ý: chỉ Admin mới gọi được. Khuyến mãi đã áp dụng cho đơn nạp
    /// sẽ được snapshot trong FxSnapshot của deposit_orders → không mất dữ liệu lịch sử.
    /// </summary>
    public async Task DeleteAsync(DepositPromotion promotion, CancellationToken cancellationToken = default)
    {
        _context.DepositPromotions.Remove(promotion);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
