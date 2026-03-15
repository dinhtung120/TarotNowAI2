using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiagController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public DiagController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("wipe")]
    public async Task<IActionResult> Wipe()
    {
        var logs = new List<string>();
        try 
        {
            var conn = _dbContext.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync();
            logs.Add($"Connected to: {conn.Database}");

            // 1. Terminate other connections
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $@"
                    SELECT pg_terminate_backend(pg_stat_activity.pid)
                    FROM pg_stat_activity
                    WHERE pg_stat_activity.datname = '{conn.Database}'
                      AND pid <> pg_backend_pid();";
                await cmd.ExecuteNonQueryAsync();
                logs.Add("Other connections terminated.");
            }

            // 2. Drop schema public
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DROP SCHEMA public CASCADE; CREATE SCHEMA public; GRANT ALL ON SCHEMA public TO public; GRANT ALL ON SCHEMA public TO CURRENT_USER;";
                await cmd.ExecuteNonQueryAsync();
                logs.Add("Schema public dropped and recreated.");
            }

            return Ok(new { Message = "Wipe successful", Logs = logs });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Logs = logs });
        }
    }
}
