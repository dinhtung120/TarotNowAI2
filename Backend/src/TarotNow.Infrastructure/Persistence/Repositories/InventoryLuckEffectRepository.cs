using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository lưu trạng thái hiệu ứng may mắn từ inventory items.
/// </summary>
public sealed class InventoryLuckEffectRepository : IInventoryLuckEffectRepository
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo repository hiệu ứng may mắn.
    /// </summary>
    public InventoryLuckEffectRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task ApplyLuckAsync(
        Guid userId,
        int luckValue,
        string sourceItemCode,
        TimeSpan duration,
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (luckValue <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(luckValue), "Luck value must be greater than zero.");
        }

        var effect = await _dbContext.Set<InventoryLuckEffect>()
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (effect is null)
        {
            effect = new InventoryLuckEffect(userId, luckValue, sourceItemCode, duration);
            await _dbContext.Set<InventoryLuckEffect>().AddAsync(effect, cancellationToken);
        }
        else
        {
            effect.Apply(luckValue, sourceItemCode, duration);
            _dbContext.Set<InventoryLuckEffect>().Update(effect);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
