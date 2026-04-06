using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoLeaderboardRepository : ILeaderboardRepository
{
    private readonly MongoDbContext _context;

    public MongoLeaderboardRepository(MongoDbContext context)
    {
        _context = context;
    }
}
