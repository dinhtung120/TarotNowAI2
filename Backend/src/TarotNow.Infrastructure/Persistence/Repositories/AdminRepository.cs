

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

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
            WHERE u.gold_balance IS DISTINCT FROM COALESCE(v.ledger_gold, 0)
               OR u.diamond_balance IS DISTINCT FROM COALESCE(v.ledger_diamond, 0);
        ";

        
        return await _dbContext.Database.SqlQueryRaw<MismatchRecord>(sql)
                               .ToListAsync(cancellationToken);
    }
}
