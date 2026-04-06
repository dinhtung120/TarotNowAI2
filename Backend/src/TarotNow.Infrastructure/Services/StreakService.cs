using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure.Services;

/// <summary>
/// Đại nội tổng quản lo việc Cộng hoặc Đứt chuỗi đăng nhập.
/// Tách rời khỏi Handler để dùng chung được ở nhiều nơi nếu Phase sau mọc thêm nguồn nhận rút bài (VD: Friend Chain).
/// </summary>
public class StreakService : IStreakService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<StreakService> _logger;

    public StreakService(IUserRepository userRepository, ILogger<StreakService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task IncrementStreakOnValidDrawAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("[StreakService] Không tìm thấy User {UserId} khi tính Streak.", userId);
            return;
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Đã cộng trong ngày hôm nay rồi thì thôi bỏ.
        if (user.LastStreakDate.HasValue && user.LastStreakDate.Value == today)
        {
            return;
        }

        try
        {
            if (!user.LastStreakDate.HasValue)
            {
                // Chưa bao giờ rút (Lính mới)
                user.IncrementStreak(today);
                _logger.LogInformation("[StreakService] User {UserId} lính mới, bắt đầu Streak = {Streak}.", userId, user.CurrentStreak);
            }
            else
            {
                // Kiểm tra xem có bị thủng đoạn ở giữa không (hôm qua chưa rút)
                var yesterday = today.AddDays(-1);
                
                if (user.LastStreakDate.Value == yesterday)
                {
                    // Liền tù tì
                    user.IncrementStreak(today);
                    _logger.LogInformation("[StreakService] User {UserId} streak liền mạch = {Streak}.", userId, user.CurrentStreak);
                }
                else if (user.LastStreakDate.Value < yesterday)
                {
                    // Đứt xích. Gọi chém Streak làm Freeze mồi, sau đó bắt đầu điểm 1.
                    _logger.LogInformation("[StreakService] User {UserId} bị đứt Streak (Từ {LastDate} tới {Today}). Cắt chuỗi.", 
                        userId, user.LastStreakDate.Value, today);
                        
                    user.BreakStreak();
                    user.IncrementStreak(today);
                }
            }

            await _userRepository.UpdateAsync(user, cancellationToken);
            _logger.LogInformation("[StreakService] ✅ Save Streak thành công cho User {UserId}. CurrentStreak={Streak}, LastDate={Date}.", 
                userId, user.CurrentStreak, user.LastStreakDate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[StreakService] Lỗi tèo khi tăng Streak cho User {UserId}.", userId);
        }
    }
}
