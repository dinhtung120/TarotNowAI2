using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

// Adapter đọc cấu hình JWT và trả về giá trị đã chuẩn hóa cho tầng Application.
public sealed class JwtTokenSettings : IJwtTokenSettings
{
    /// <summary>
    /// Khởi tạo settings JWT với fallback an toàn khi cấu hình thiếu hoặc sai.
    /// Luồng chuẩn hóa giúp tránh token sống quá ngắn/dài do dữ liệu cấu hình không hợp lệ.
    /// </summary>
    public JwtTokenSettings(IOptions<JwtOptions> options)
    {
        var jwtOptions = options.Value;
        // Chuẩn hóa thời hạn access token để policy xác thực luôn có giá trị dương.
        AccessTokenExpiryMinutes = ResolvePositiveInt(
            jwtOptions.ExpiryMinutes.ToString(),
            fallback: 15);

        // Chuẩn hóa thời hạn refresh token để luồng làm mới phiên không bị cấu hình âm/0.
        RefreshTokenExpiryDays = ResolvePositiveInt(
            jwtOptions.RefreshExpiryDays.ToString(),
            fallback: 7);
    }

    // Số phút sống của access token sau khi chuẩn hóa.
    public int AccessTokenExpiryMinutes { get; }
    // Số ngày sống của refresh token sau khi chuẩn hóa.
    public int RefreshTokenExpiryDays { get; }

    /// <summary>
    /// Chuẩn hóa chuỗi cấu hình thành số nguyên dương.
    /// Luồng fallback giữ hệ thống hoạt động ổn định khi dữ liệu cấu hình không parse được.
    /// </summary>
    private static int ResolvePositiveInt(string? configuredValue, int fallback)
    {
        return int.TryParse(configuredValue, out var parsed) && parsed > 0 ? parsed : fallback;
    }
}
