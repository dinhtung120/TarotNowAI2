using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoQuestRepository : IQuestRepository
{
    private readonly MongoDbContext _context;

    public MongoQuestRepository(MongoDbContext context)
    {
        _context = context;
    }
}
