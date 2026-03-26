namespace TarotNow.Infrastructure.Options;

public sealed class DiagnosticsOptions
{
    public SeedAdminOptions SeedAdmin { get; set; } = new();

    public sealed class SeedAdminOptions
    {
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
