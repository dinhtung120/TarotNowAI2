using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository quản lý credit free draw của người dùng.
/// </summary>
public sealed class FreeDrawCreditRepository : IFreeDrawCreditRepository
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo repository free draw credit.
    /// </summary>
    public FreeDrawCreditRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task AddCreditsAsync(
        Guid userId,
        int spreadCardCount,
        int creditCount,
        CancellationToken cancellationToken = default)
    {
        ValidateAddCreditsInput(userId, spreadCardCount, creditCount);
        var nowUtc = DateTime.UtcNow;
        var newId = Guid.CreateVersion7();
        await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             INSERT INTO free_draw_credits
                 (id, user_id, spread_card_count, available_count, created_at_utc, updated_at_utc)
             VALUES
                 ({newId}, {userId}, {spreadCardCount}, {creditCount}, {nowUtc}, {nowUtc})
             ON CONFLICT (user_id, spread_card_count)
             DO UPDATE SET
                 available_count = free_draw_credits.available_count + EXCLUDED.available_count,
                 updated_at_utc = EXCLUDED.updated_at_utc;
             """,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> TryConsumeAsync(Guid userId, int spreadCardCount, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (spreadCardCount is not (3 or 5 or 10))
        {
            throw new ArgumentOutOfRangeException(nameof(spreadCardCount), "Spread card count must be one of 3, 5, 10.");
        }

        var nowUtc = DateTime.UtcNow;
        var affectedRows = await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            $"""
             UPDATE free_draw_credits
             SET available_count = available_count - 1,
                 updated_at_utc = {nowUtc}
             WHERE user_id = {userId}
               AND spread_card_count = {spreadCardCount}
               AND available_count > 0;
             """,
            cancellationToken);
        return affectedRows > 0;
    }

    /// <inheritdoc />
    public async Task<FreeDrawCreditSummary> GetSummaryAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        var items = await _dbContext.Set<FreeDrawCredit>()
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.AvailableCount > 0)
            .ToListAsync(cancellationToken);

        var spread3 = items.FirstOrDefault(x => x.SpreadCardCount == 3)?.AvailableCount ?? 0;
        var spread5 = items.FirstOrDefault(x => x.SpreadCardCount == 5)?.AvailableCount ?? 0;
        var spread10 = items.FirstOrDefault(x => x.SpreadCardCount == 10)?.AvailableCount ?? 0;
        return new FreeDrawCreditSummary
        {
            Spread3Count = spread3,
            Spread5Count = spread5,
            Spread10Count = spread10,
        };
    }

    private static void ValidateAddCreditsInput(Guid userId, int spreadCardCount, int creditCount)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (spreadCardCount is not (3 or 5 or 10))
        {
            throw new ArgumentOutOfRangeException(nameof(spreadCardCount), "Spread card count must be one of 3, 5, 10.");
        }

        if (creditCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(creditCount), "Credit count must be greater than zero.");
        }
    }
}
