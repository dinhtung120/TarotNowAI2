namespace TarotNow.Application.Interfaces;

/// <summary>
/// Cấu hình tuổi thọ token cho luồng xác thực.
/// Tách thành abstraction để Application không phụ thuộc IConfiguration.
/// </summary>
public interface IJwtTokenSettings
{
    int AccessTokenExpiryMinutes { get; }
    int RefreshTokenExpiryDays { get; }
}
