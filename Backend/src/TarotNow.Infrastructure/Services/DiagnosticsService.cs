using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Services;

public sealed class DiagnosticsService : IDiagnosticsService
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

    public async Task<SeedAdminResult> SeedAdminAsync(CancellationToken cancellationToken = default)
    {
        var adminEmail = _configuration["Diagnostics:SeedAdmin:Email"]?.Trim();
        var adminUsername = _configuration["Diagnostics:SeedAdmin:Username"]?.Trim();
        var adminPassword = _configuration["Diagnostics:SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) ||
            string.IsNullOrWhiteSpace(adminUsername) ||
            string.IsNullOrWhiteSpace(adminPassword) ||
            adminPassword.Length < 12)
        {
            return new SeedAdminResult
            {
                Status = SeedAdminStatus.InvalidConfiguration,
                Message = "Missing diagnostics seed admin config. Set Diagnostics:SeedAdmin:{Email,Username,Password} with strong password."
            };
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == adminEmail, cancellationToken);
        var passwordHash = _passwordHasher.HashPassword(adminPassword);
        var isNew = false;

        if (user == null)
        {
            isNew = true;
            user = new User(
                adminEmail,
                adminUsername,
                passwordHash,
                "Super Admin",
                new DateTime(1985, 5, 5).ToUniversalTime(),
                true);

            user.Activate();
            user.PromoteToAdmin();
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }
        else
        {
            user.PromoteToAdmin();
            user.Activate();
            user.UpdatePassword(passwordHash);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new SeedAdminResult
        {
            Status = SeedAdminStatus.Success,
            Message = isNew ? "SuperAdmin created" : "SuperAdmin updated",
            Email = adminEmail,
            Username = adminUsername
        };
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
