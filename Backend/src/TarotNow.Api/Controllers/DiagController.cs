using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "admin")]
[ApiExplorerSettings(IgnoreApi = true)]
public class DiagController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DiagController> _logger;

    public DiagController(
        ApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        IWebHostEnvironment environment,
        IConfiguration configuration,
        ILogger<DiagController> logger)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _environment = environment;
        _configuration = configuration;
        _logger = logger;
    }

    private IActionResult? RejectIfNotDevelopment()
    {
        if (_environment.IsDevelopment()) return null;
        return NotFound();
    }

    [HttpPost("wipe")]
    public IActionResult Wipe()
    {
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard;

        return Ok(new { message = "Wipe endpoint is disabled by default." });
    }

    [HttpPost("seed-admin")]
    public async Task<IActionResult> SeedAdmin()
    {
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard;

        try 
        {
            var adminEmail = _configuration["Diagnostics:SeedAdmin:Email"]?.Trim();
            var adminUsername = _configuration["Diagnostics:SeedAdmin:Username"]?.Trim();
            var adminPassword = _configuration["Diagnostics:SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(adminEmail) ||
                string.IsNullOrWhiteSpace(adminUsername) ||
                string.IsNullOrWhiteSpace(adminPassword) ||
                adminPassword.Length < 12)
            {
                return BadRequest(new
                {
                    message = "Missing diagnostics seed admin config. Set Diagnostics:SeedAdmin:{Email,Username,Password} with strong password."
                });
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
            var passwordHash = _passwordHasher.HashPassword(adminPassword);

            bool isNew = false;
            if (user == null)
            {
                isNew = true;
                user = new TarotNow.Domain.Entities.User(
                    adminEmail, 
                    adminUsername,
                    passwordHash, 
                    "Super Admin", 
                    new DateTime(1985, 5, 5).ToUniversalTime(), 
                    true
                );
                user.Activate();
                user.PromoteToAdmin();
                
                await _dbContext.Users.AddAsync(user);
            }
            else
            {
                user.PromoteToAdmin();
                user.Activate();
                user.UpdatePassword(passwordHash);
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new { 
                Message = isNew ? "SuperAdmin created" : "SuperAdmin updated", 
                Email = adminEmail, 
                Username = adminUsername
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to seed admin account");
            return StatusCode(500, new { message = "Failed to seed admin account." });
        }
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats([FromServices] TarotNow.Infrastructure.Persistence.MongoDbContext mongoContext)
    {
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard;

        try
        {
            var totalSessions = await mongoContext.ReadingSessions.CountDocumentsAsync(new BsonDocument());
            var testUserId = "c6f6ca4e-042d-44c8-8812-bdce1b4b1563";
            var testUserSessions = await mongoContext.ReadingSessions.CountDocumentsAsync(
                Builders<TarotNow.Infrastructure.Persistence.MongoDocuments.ReadingSessionDocument>.Filter.Eq(r => r.UserId, testUserId)
            );

            var sampleDocs = await mongoContext.ReadingSessions
                .Find(new BsonDocument())
                .Limit(5)
                .ToListAsync();

            var rawJsonSamples = sampleDocs.Select(d => d.ToJson()).ToList();

            return Ok(new { 
                TotalSessionsInMongo = totalSessions,
                TestUserSessions = testUserSessions,
                SampleDataRaw = rawJsonSamples
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch diagnostics stats");
            return StatusCode(500, new { message = "Failed to fetch diagnostics stats." });
        }
    }
}
