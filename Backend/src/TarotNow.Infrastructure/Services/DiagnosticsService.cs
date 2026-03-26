using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Services;

public sealed partial class DiagnosticsService : IDiagnosticsService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly MongoDbContext _mongoContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DiagnosticsService> _logger;

    public DiagnosticsService(
        ApplicationDbContext dbContext,
        MongoDbContext mongoContext,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        ILogger<DiagnosticsService> logger)
    {
        _dbContext = dbContext;
        _mongoContext = mongoContext;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<DiagnosticsStatsResult> GetStatsAsync(CancellationToken cancellationToken = default)
    {
        const string testUserId = "c6f6ca4e-042d-44c8-8812-bdce1b4b1563";

        var totalSessions = await _mongoContext.ReadingSessions.CountDocumentsAsync(new BsonDocument(), cancellationToken: cancellationToken);
        var testUserSessions = await _mongoContext.ReadingSessions.CountDocumentsAsync(
            Builders<ReadingSessionDocument>.Filter.Eq(r => r.UserId, testUserId),
            cancellationToken: cancellationToken);

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
}
