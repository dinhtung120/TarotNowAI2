namespace TarotNow.Application.Interfaces;

public interface IDiagnosticsService
{
    Task<SeedAdminResult> SeedAdminAsync(CancellationToken cancellationToken = default);
    Task SeedGamificationDataAsync(CancellationToken cancellationToken = default);
    Task<DiagnosticsStatsResult> GetStatsAsync(CancellationToken cancellationToken = default);
}

public enum SeedAdminStatus
{
    Success = 1,
    InvalidConfiguration = 2
}

public sealed class SeedAdminResult
{
    public SeedAdminStatus Status { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Username { get; init; }
}

public sealed class DiagnosticsStatsResult
{
    public long TotalSessionsInMongo { get; init; }
    public long TestUserSessions { get; init; }
    public List<string> SampleDataRaw { get; init; } = new();
}
