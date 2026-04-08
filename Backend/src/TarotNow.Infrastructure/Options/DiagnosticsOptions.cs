namespace TarotNow.Infrastructure.Options;

// Options cấu hình nghiệp vụ diagnostics nội bộ.
public sealed class DiagnosticsOptions
{
    // Cấu hình seed tài khoản admin.
    public SeedAdminOptions SeedAdmin { get; set; } = new();

    // Cấu hình chi tiết tài khoản admin seed.
    public sealed class SeedAdminOptions
    {
        // Email tài khoản admin seed.
        public string Email { get; set; } = string.Empty;

        // Username tài khoản admin seed.
        public string Username { get; set; } = string.Empty;

        // Mật khẩu tài khoản admin seed.
        public string Password { get; set; } = string.Empty;
    }
}
