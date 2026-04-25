using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Repository điểm danh ngày trên MongoDB.
public class MongoDailyCheckinRepository : IDailyCheckinRepository
{
    // Mongo context truy cập collection daily_checkins.
    private readonly MongoDbContext _context;

    /// <summary>
    /// Khởi tạo repository daily check-in.
    /// Luồng xử lý: nhận MongoDbContext từ DI để thao tác dữ liệu check-in.
    /// </summary>
    public MongoDailyCheckinRepository(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Kiểm tra user đã check-in trong business date hay chưa.
    /// Luồng xử lý: count documents theo cặp userId-businessDate.
    /// </summary>
    public async Task<bool> HasCheckedInAsync(string userId, string businessDate, CancellationToken cancellationToken = default)
    {
        var filter = Builders<DailyCheckinDocument>.Filter.And(
            Builders<DailyCheckinDocument>.Filter.Eq(x => x.UserId, userId),
            Builders<DailyCheckinDocument>.Filter.Eq(x => x.BusinessDate, businessDate)
        );

        var count = await _context.DailyCheckins.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        return count > 0;
    }

    /// <summary>
    /// Ghi nhận một lượt check-in mới theo hướng insert-first.
    /// Luồng xử lý: insert document mới; nếu trùng unique key userId-businessDate thì trả false để caller xử lý idempotent.
    /// </summary>
    public async Task<bool> TryInsertAsync(string userId, string businessDate, long goldReward, CancellationToken cancellationToken = default)
    {
        var document = new DailyCheckinDocument
        {
            UserId = userId,
            BusinessDate = businessDate,
            GoldReward = goldReward,
            CreatedAt = System.DateTime.UtcNow
        };

        try
        {
            await _context.DailyCheckins.InsertOneAsync(document, cancellationToken: cancellationToken);
            return true;
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }

    /// <summary>
    /// Đếm số lượt check-in trong N ngày gần nhất.
    /// Luồng xử lý: tính minDate theo UTC, lọc user + businessDate >= minDate rồi count.
    /// </summary>
    public async Task<int> GetCheckinCountAsync(string userId, int recentDays, CancellationToken cancellationToken = default)
    {
        var minDate = System.DateOnly.FromDateTime(System.DateTime.UtcNow.AddDays(-recentDays)).ToString("yyyy-MM-dd");
        // BusinessDate lưu dạng chuỗi yyyy-MM-dd nên so sánh GTE theo chuỗi vẫn đúng thứ tự thời gian.

        var filter = Builders<DailyCheckinDocument>.Filter.And(
            Builders<DailyCheckinDocument>.Filter.Eq(x => x.UserId, userId),
            Builders<DailyCheckinDocument>.Filter.Gte(x => x.BusinessDate, minDate)
        );

        var count = await _context.DailyCheckins.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        return (int)count;
    }
}
