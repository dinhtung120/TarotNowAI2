using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Repositories;

public partial class ChatFinanceRepository : IChatFinanceRepository
{
    private readonly ApplicationDbContext _db;

    public ChatFinanceRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
    }
}
