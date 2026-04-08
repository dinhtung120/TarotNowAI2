namespace TarotNow.Application.Interfaces;

// Contract cấu hình thời hạn token JWT để dùng thống nhất trong luồng xác thực.
public interface IJwtTokenSettings
{
    // Thời gian sống của access token (phút) để cân bằng bảo mật và trải nghiệm.
    int AccessTokenExpiryMinutes { get; }

    // Thời gian sống của refresh token (ngày) để điều khiển vòng đời phiên đăng nhập dài hạn.
    int RefreshTokenExpiryDays { get; }
}
