using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Services;

public sealed partial class DiagnosticsService
{
    // Cấu hình tối thiểu để seed tài khoản admin phục vụ môi trường vận hành/kiểm thử.
    private sealed record SeedAdminConfig(string Email, string Username, string Password);

    /// <summary>
    /// Seed hoặc cập nhật tài khoản Super Admin theo cấu hình Diagnostics.
    /// Luồng đọc cấu hình, validate điều kiện an toàn, upsert tài khoản rồi commit một lần.
    /// </summary>
    public async Task<SeedAdminResult> SeedAdminAsync(CancellationToken cancellationToken = default)
    {
        if (!TryReadSeedAdminConfig(out var config))
        {
            // Cấu hình thiếu hoặc yếu thì dừng sớm để tránh tạo tài khoản không an toàn.
            return BuildInvalidConfigResult();
        }

        var isNew = await UpsertAdminAccountAsync(config!, cancellationToken);
        // Commit sau khi upsert để đồng bộ trạng thái admin trong DB quan hệ.
        await _dbContext.SaveChangesAsync(cancellationToken);
        return BuildSuccessResult(config!, isNew);
    }

    /// <summary>
    /// Đọc và kiểm tra cấu hình seed admin từ appsettings.
    /// Luồng yêu cầu email, username, password hợp lệ để tránh seed tài khoản rỗng/yếu.
    /// </summary>
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
            // Rule tối thiểu: password >= 12 để giảm rủi ro tài khoản mặc định bị đoán.
            config = null;
            return false;
        }

        // Trả về cấu hình đã chuẩn hóa để dùng cho nhánh upsert tiếp theo.
        config = new SeedAdminConfig(email, username, password);
        return true;
    }

    /// <summary>
    /// Upsert tài khoản admin theo email cấu hình.
    /// Luồng cập nhật user cũ nếu tồn tại, ngược lại tạo user mới rồi gán vai trò admin.
    /// </summary>
    private async Task<bool> UpsertAdminAccountAsync(SeedAdminConfig config, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == config.Email, cancellationToken);
        var passwordHash = _passwordHasher.HashPassword(config.Password);
        if (user != null)
        {
            // Nâng quyền + kích hoạt + cập nhật mật khẩu để đảm bảo account cũ đúng chuẩn admin hiện tại.
            user.PromoteToAdmin();
            user.Activate();
            user.UpdatePassword(passwordHash);
            return false;
        }

        // Không tồn tại user theo email thì tạo mới và chuẩn hóa quyền/kích hoạt ngay.
        var adminUser = BuildNewAdminUser(config, passwordHash);
        adminUser.Activate();
        adminUser.PromoteToAdmin();
        await _dbContext.Users.AddAsync(adminUser, cancellationToken);
        return true;
    }

    /// <summary>
    /// Tạo thực thể người dùng admin mới với thông tin seed đã chuẩn hóa.
    /// Luồng tách builder để giảm lặp và thống nhất dữ liệu mặc định khi khởi tạo.
    /// </summary>
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

    /// <summary>
    /// Tạo kết quả phản hồi khi cấu hình seed admin không hợp lệ.
    /// Luồng tách message giúp caller hiển thị hướng dẫn cấu hình rõ ràng.
    /// </summary>
    private static SeedAdminResult BuildInvalidConfigResult()
    {
        return new SeedAdminResult
        {
            Status = SeedAdminStatus.InvalidConfiguration,
            Message = "Missing diagnostics seed admin config. Set Diagnostics:SeedAdmin:{Email,Username,Password} with strong password."
        };
    }

    /// <summary>
    /// Tạo kết quả phản hồi thành công cho thao tác seed admin.
    /// Luồng trả trạng thái tạo mới hay cập nhật để dễ theo dõi vận hành.
    /// </summary>
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
