using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class UserCollectionRepository : IUserCollectionRepository
{
    private readonly ApplicationDbContext _context;

    public UserCollectionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpsertCardAsync(Guid userId, int cardId, long expToGain, CancellationToken cancellationToken = default)
    {
        // Do tính chất có thể rút nhiều thẻ cùng 1 lúc (bất đồng bộ Race condition)
        // Nên dùng Upsert nếu Database hỗ trợ, hoặc Entity Framework tìm trước update sau trong cùng transaction.
        // Ở Postgres tối ưu nhất là dùng StoredProc hoặc SQL Raw: INSERT ... ON CONFLICT (user_id, card_id) DO UPDATE
        
        var sql = @"
            INSERT INTO user_collections (user_id, card_id, level, copies, exp_gained, last_drawn_at) 
            VALUES ({0}, {1}, 1, 1, {2}, CURRENT_TIMESTAMP)
            ON CONFLICT (user_id, card_id) DO UPDATE SET 
                copies = user_collections.copies + 1,
                exp_gained = user_collections.exp_gained + {2},
                last_drawn_at = CURRENT_TIMESTAMP,
                level = CASE WHEN (user_collections.copies + 1) % 5 = 0 THEN user_collections.level + 1 ELSE user_collections.level END;
        ";

        await _context.Database.ExecuteSqlRawAsync(sql, userId, cardId, expToGain);
    }

    public async Task<IEnumerable<UserCollection>> GetUserCollectionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserCollections
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Level)
            .ThenByDescending(x => x.Copies)
            .ToListAsync(cancellationToken);
    }
}
