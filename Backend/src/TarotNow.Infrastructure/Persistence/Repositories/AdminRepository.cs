using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AdminRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MismatchRecord>> GetLedgerMismatchesAsync(CancellationToken cancellationToken = default)
    {
        var sql = @"
            SELECT u.id AS UserId, u.gold_balance AS UserGoldBalance, v.ledger_gold AS LedgerGoldBalance, 
                   u.diamond_balance AS UserDiamondBalance, v.ledger_diamond AS LedgerDiamondBalance 
            FROM users u
            LEFT JOIN v_user_ledger_balance v ON u.id = v.user_id
            WHERE u.gold_balance != v.ledger_gold OR u.diamond_balance != v.ledger_diamond;
        ";

        // Query raw SQL Mapping to Dto using EF Core
        return await _dbContext.Database.SqlQueryRaw<MismatchRecord>(sql)
                               .ToListAsync(cancellationToken);
    }
}
