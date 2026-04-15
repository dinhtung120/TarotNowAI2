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
    public async Task AddCreditsAsync(Guid userId, int creditCount, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (creditCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(creditCount), "Credit count must be greater than zero.");
        }

        var credit = await _dbContext.Set<FreeDrawCredit>()
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (credit is null)
        {
            credit = new FreeDrawCredit(userId, creditCount);
            await _dbContext.Set<FreeDrawCredit>().AddAsync(credit, cancellationToken);
        }
        else
        {
            credit.AddCredits(creditCount);
            _dbContext.Set<FreeDrawCredit>().Update(credit);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
