

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository quản lý cấu hình promotion nạp tiền.
public class DepositPromotionRepository : IDepositPromotionRepository
{
    // DbContext thao tác bảng deposit_promotions.
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Khởi tạo repository promotion.
    /// Luồng xử lý: nhận DbContext từ DI để dùng xuyên suốt các command/query admin.
    /// </summary>
    public DepositPromotionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy promotion theo id.
    /// Luồng xử lý: query bản ghi đầu tiên khớp id hoặc null nếu không tồn tại.
    /// </summary>
    public async Task<DepositPromotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DepositPromotions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Lấy toàn bộ promotion theo thứ tự ngưỡng nạp giảm dần.
    /// Luồng xử lý: sort MinAmountVnd desc để luồng matching ưu tiên ngưỡng cao trước.
    /// </summary>
    public async Task<IEnumerable<DepositPromotion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DepositPromotions
            .OrderByDescending(p => p.MinAmountVnd)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Lấy các promotion đang active.
    /// Luồng xử lý: lọc IsActive=true rồi sắp theo ngưỡng nạp giảm dần.
    /// </summary>
    public async Task<IEnumerable<DepositPromotion>> GetActivePromotionsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DepositPromotions
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.MinAmountVnd)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Thêm mới promotion.
    /// Luồng xử lý: add entity và save để bản ghi có hiệu lực ngay.
    /// </summary>
    public async Task AddAsync(DepositPromotion promotion, CancellationToken cancellationToken = default)
    {
        await _context.DepositPromotions.AddAsync(promotion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Cập nhật promotion hiện có.
    /// Luồng xử lý: mark modified và persist vào DB.
    /// </summary>
    public async Task UpdateAsync(DepositPromotion promotion, CancellationToken cancellationToken = default)
    {
        _context.DepositPromotions.Update(promotion);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Xóa promotion.
    /// Luồng xử lý: remove entity rồi save để loại khỏi tập áp dụng nghiệp vụ.
    /// </summary>
    public async Task DeleteAsync(DepositPromotion promotion, CancellationToken cancellationToken = default)
    {
        _context.DepositPromotions.Remove(promotion);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
