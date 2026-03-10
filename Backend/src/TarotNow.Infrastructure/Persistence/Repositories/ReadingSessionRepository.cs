using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class ReadingSessionRepository : IReadingSessionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IWalletRepository _walletRepository;

    public ReadingSessionRepository(ApplicationDbContext context, IWalletRepository walletRepository)
    {
        _context = context;
        _walletRepository = walletRepository; // Tái sử dụng để gọi Stored Procedure trừ tiền
    }

    public async Task<ReadingSession> CreateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        await _context.ReadingSessions.AddAsync(session, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return session;
    }

    public async Task<ReadingSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ReadingSessions.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default)
    {
        _context.ReadingSessions.Update(session);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> HasDrawnDailyCardAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var startOfDayUtc = utcNow.Date;
        var endOfDayUtc = startOfDayUtc.AddDays(1);

        return await _context.ReadingSessions.AnyAsync(s => 
            s.UserId == userId && 
            s.SpreadType == SpreadType.Daily1Card &&
            s.CreatedAt >= startOfDayUtc && 
            s.CreatedAt < endOfDayUtc, 
            cancellationToken);
    }

    public async Task<(bool Success, string ErrorMessage)> StartPaidSessionAtomicAsync(
        Guid userId, 
        string spreadType, 
        ReadingSession session, 
        long costGold, 
        long costDiamond, 
        CancellationToken cancellationToken = default)
    {
        // Sử dụng ID session làm IdempotencyKey chống trừ tiền đúp (Double-spending)
        var idempotencyKey = $"read_{session.Id}";

        // EF Core Transaction để đảm bảo tính Acid (Tạo Entity Session & Trừ Tiền SP cùng 1 lượt)
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 1. Trừ tiền bằng Stored Procedure
            if (costGold > 0)
            {
                await _walletRepository.DebitAsync(userId, CurrencyType.Gold, TransactionType.ReadingCostGold, costGold, "Reading", $"Tarot_{spreadType}", $"Phiên rút Tarot {spreadType}", null, idempotencyKey, cancellationToken);
            }
            if (costDiamond > 0)
            {
                await _walletRepository.DebitAsync(userId, CurrencyType.Diamond, TransactionType.ReadingCostDiamond, costDiamond, "Reading", $"Tarot_{spreadType}", $"Phiên rút Tarot {spreadType}", null, idempotencyKey, cancellationToken);
            }

            // 2. Tạo Session DB
            await _context.ReadingSessions.AddAsync(session, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // 3. Commit
            await transaction.CommitAsync(cancellationToken);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            // Có thể do lỗi số dư không đủ từ Exception của proc_wallet_debit
            return (false, ex.InnerException?.Message ?? ex.Message);
        }
    }
}
