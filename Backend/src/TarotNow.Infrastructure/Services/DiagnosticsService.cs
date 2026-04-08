using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Options;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using TarotNow.Infrastructure.Persistence.Seeds;

namespace TarotNow.Infrastructure.Services;

// Service diagnostics hỗ trợ thống kê nhanh và seed dữ liệu vận hành.
public sealed partial class DiagnosticsService : IDiagnosticsService
{
    // DbContext quan hệ để đọc/ghi dữ liệu chẩn đoán liên quan SQL.
    private readonly ApplicationDbContext _dbContext;
    // MongoDbContext để thống kê dữ liệu phiên đọc realtime.
    private readonly MongoDbContext _mongoContext;
    // Hasher phục vụ luồng seed admin an toàn.
    private readonly IPasswordHasher _passwordHasher;
    // Cấu hình diagnostics cho các tác vụ seed/kiểm tra.
    private readonly DiagnosticsOptions _options;
    // Logger theo dõi thao tác diagnostics.
    private readonly ILogger<DiagnosticsService> _logger;

    /// <summary>
    /// Khởi tạo service diagnostics với đầy đủ dependency lưu trữ và cấu hình.
    /// Luồng này gom phụ thuộc tại một điểm để các partial xử lý thống nhất.
    /// </summary>
    public DiagnosticsService(
        ApplicationDbContext dbContext,
        MongoDbContext mongoContext,
        IPasswordHasher passwordHasher,
        IOptions<DiagnosticsOptions> options,
        ILogger<DiagnosticsService> logger)
    {
        _dbContext = dbContext;
        _mongoContext = mongoContext;
        _passwordHasher = passwordHasher;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Lấy thống kê nhanh dữ liệu đọc bài từ MongoDB cho mục đích chẩn đoán.
    /// Luồng trả tổng số phiên, số phiên của user mẫu và một ít dữ liệu thô để kiểm tra tình trạng lưu trữ.
    /// </summary>
    public async Task<DiagnosticsStatsResult> GetStatsAsync(CancellationToken cancellationToken = default)
    {
        // User mẫu cố định để đội vận hành đối chiếu nhanh dữ liệu test.
        const string testUserId = "c6f6ca4e-042d-44c8-8812-bdce1b4b1563";

        // Đếm tổng phiên toàn bộ collection để phản ánh quy mô dữ liệu hiện tại.
        var totalSessions = await _mongoContext.ReadingSessions.CountDocumentsAsync(new BsonDocument(), cancellationToken: cancellationToken);
        var testUserSessions = await _mongoContext.ReadingSessions.CountDocumentsAsync(
            Builders<ReadingSessionDocument>.Filter.Eq(r => r.UserId, testUserId),
            cancellationToken: cancellationToken);

        // Lấy mẫu 5 bản ghi đầu để kiểm tra nhanh cấu trúc document trong thực tế.
        var sampleDocs = await _mongoContext.ReadingSessions
            .Find(new BsonDocument())
            .Limit(5)
            .ToListAsync(cancellationToken);

        return new DiagnosticsStatsResult
        {
            TotalSessionsInMongo = totalSessions,
            TestUserSessions = testUserSessions,
            SampleDataRaw = sampleDocs.Select(d => d.ToJson()).ToList()
        };
    }

    /// <summary>
    /// Seed dữ liệu gamification mặc định vào MongoDB.
    /// Luồng này tách riêng để hỗ trợ bootstrap môi trường mới nhanh hơn.
    /// </summary>
    public Task SeedGamificationDataAsync(CancellationToken cancellationToken = default)
    {
        return SeedGamificationData.SeedAsync(_mongoContext);
    }
}
