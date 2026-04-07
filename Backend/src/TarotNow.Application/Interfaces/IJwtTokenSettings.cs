namespace TarotNow.Application.Interfaces;

public interface IJwtTokenSettings
{
    int AccessTokenExpiryMinutes { get; }
    int RefreshTokenExpiryDays { get; }
}
