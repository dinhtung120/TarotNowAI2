using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class MongoDailyCheckinRepository : IDailyCheckinRepository
{
    private readonly MongoDbContext _context;

    public MongoDailyCheckinRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasCheckedInAsync(string userId, string businessDate, CancellationToken cancellationToken = default)
    {
        var filter = Builders<DailyCheckinDocument>.Filter.And(
            Builders<DailyCheckinDocument>.Filter.Eq(x => x.UserId, userId),
            Builders<DailyCheckinDocument>.Filter.Eq(x => x.BusinessDate, businessDate)
        );

        var count = await _context.DailyCheckins.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        return count > 0;
    }

    public async Task InsertAsync(string userId, string businessDate, long goldReward, CancellationToken cancellationToken = default)
    {
        var document = new DailyCheckinDocument
        {
            UserId = userId,
            BusinessDate = businessDate,
            GoldReward = goldReward,
            CreatedAt = System.DateTime.UtcNow
        };

        // Safe insert để nếu trùng index unique thì kệ nó lẳng lặng nuốt lỗi hoặc ném ra Application Exception tuỳ business
        try
        {
            await _context.DailyCheckins.InsertOneAsync(document, cancellationToken: cancellationToken);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            // Trùng khoá = Thằng này bấm 2 lần cùng một ngày. Xoè kèo.
            throw new System.InvalidOperationException("Hôm nay bạn đã điểm danh rồi, đừng cố bấm nữa.");
        }
    }

    public async Task<int> GetCheckinCountAsync(string userId, int recentDays, CancellationToken cancellationToken = default)
    {
        // Tìm 7 ngày đổ lại
        var minDate = System.DateOnly.FromDateTime(System.DateTime.UtcNow.AddDays(-recentDays)).ToString("yyyy-MM-dd");

        var filter = Builders<DailyCheckinDocument>.Filter.And(
            Builders<DailyCheckinDocument>.Filter.Eq(x => x.UserId, userId),
            Builders<DailyCheckinDocument>.Filter.Gte(x => x.BusinessDate, minDate)
        );

        var count = await _context.DailyCheckins.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        return (int)count;
    }
}
