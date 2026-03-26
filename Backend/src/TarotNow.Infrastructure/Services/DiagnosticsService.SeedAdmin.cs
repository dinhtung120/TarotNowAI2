using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Services;

public sealed partial class DiagnosticsService
{
    private sealed record SeedAdminConfig(string Email, string Username, string Password);

    public async Task<SeedAdminResult> SeedAdminAsync(CancellationToken cancellationToken = default)
    {
        if (!TryReadSeedAdminConfig(out var config))
        {
            return BuildInvalidConfigResult();
        }

        var isNew = await UpsertAdminAccountAsync(config!, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return BuildSuccessResult(config!, isNew);
    }

    private bool TryReadSeedAdminConfig(out SeedAdminConfig? config)
    {
        var seedAdmin = _options.SeedAdmin;
        var email = seedAdmin.Email?.Trim();
        var username = seedAdmin.Username?.Trim();
        var password = seedAdmin.Password;

        if (string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(username)
            || string.IsNullOrWhiteSpace(password)
            || password.Length < 12)
        {
            config = null;
            return false;
        }

        config = new SeedAdminConfig(email, username, password);
        return true;
    }

    private async Task<bool> UpsertAdminAccountAsync(SeedAdminConfig config, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == config.Email, cancellationToken);
        var passwordHash = _passwordHasher.HashPassword(config.Password);
        if (user != null)
        {
            user.PromoteToAdmin();
            user.Activate();
            user.UpdatePassword(passwordHash);
            return false;
        }

        var adminUser = BuildNewAdminUser(config, passwordHash);
        adminUser.Activate();
        adminUser.PromoteToAdmin();
        await _dbContext.Users.AddAsync(adminUser, cancellationToken);
        return true;
    }

    private static User BuildNewAdminUser(SeedAdminConfig config, string passwordHash)
    {
        return new User(
            config.Email,
            config.Username,
            passwordHash,
            "Super Admin",
            new DateTime(1985, 5, 5).ToUniversalTime(),
            true);
    }

    private static SeedAdminResult BuildInvalidConfigResult()
    {
        return new SeedAdminResult
        {
            Status = SeedAdminStatus.InvalidConfiguration,
            Message = "Missing diagnostics seed admin config. Set Diagnostics:SeedAdmin:{Email,Username,Password} with strong password."
        };
    }

    private static SeedAdminResult BuildSuccessResult(SeedAdminConfig config, bool isNew)
    {
        return new SeedAdminResult
        {
            Status = SeedAdminStatus.Success,
            Message = isNew ? "SuperAdmin created" : "SuperAdmin updated",
            Email = config.Email,
            Username = config.Username
        };
    }
}
