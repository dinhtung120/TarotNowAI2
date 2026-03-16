using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiagController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    private readonly TarotNow.Application.Interfaces.IPasswordHasher _passwordHasher;

    public DiagController(ApplicationDbContext dbContext, TarotNow.Application.Interfaces.IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("wipe")]
    public async Task<IActionResult> Wipe()
    {
        // ... (giữ nguyên code cũ)
        return Ok(); 
    }

    [HttpGet("seed-admin")]
    public async Task<IActionResult> SeedAdmin()
    {
        try 
        {
            var adminEmail = "superadmin@tarotnow.com";
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
            
            var password = "SuperSecret123!";
            var passwordHash = _passwordHasher.HashPassword(password); 

            bool isNew = false;
            if (user == null)
            {
                isNew = true;
                user = new TarotNow.Domain.Entities.User(
                    adminEmail, 
                    "superadmin", 
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
                Username = "superadmin",
                Password = password,
                Note = "Sử dụng Email hoặc Username để đăng nhập"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats([FromServices] TarotNow.Infrastructure.Persistence.MongoDbContext mongoContext)
    {
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
            return BadRequest(new { Error = ex.Message });
        }
    }
}
