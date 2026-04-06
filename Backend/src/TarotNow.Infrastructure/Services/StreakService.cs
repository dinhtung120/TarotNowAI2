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
        if (IsAlreadyProcessedForToday(user, today)) return;

        try
        {
            ApplyStreakTransition(user, userId, today);
            await _userRepository.UpdateAsync(user, cancellationToken);
            _logger.LogInformation("[StreakService] ✅ Save Streak thành công cho User {UserId}. CurrentStreak={Streak}, LastDate={Date}.", 
                userId, user.CurrentStreak, user.LastStreakDate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[StreakService] Lỗi tèo khi tăng Streak cho User {UserId}.", userId);
        }
    }

    private static bool IsAlreadyProcessedForToday(Domain.Entities.User user, DateOnly today)
        => user.LastStreakDate.HasValue && user.LastStreakDate.Value == today;

    private void ApplyStreakTransition(Domain.Entities.User user, Guid userId, DateOnly today)
    {
        if (!user.LastStreakDate.HasValue)
        {
            user.IncrementStreak(today);
            _logger.LogInformation("[StreakService] User {UserId} lính mới, bắt đầu Streak = {Streak}.", userId, user.CurrentStreak);
            return;
        }

        var yesterday = today.AddDays(-1);
        if (user.LastStreakDate.Value == yesterday)
        {
            user.IncrementStreak(today);
            _logger.LogInformation("[StreakService] User {UserId} streak liền mạch = {Streak}.", userId, user.CurrentStreak);
            return;
        }

        if (user.LastStreakDate.Value < yesterday)
        {
            _logger.LogInformation("[StreakService] User {UserId} bị đứt Streak (Từ {LastDate} tới {Today}). Cắt chuỗi.",
                userId, user.LastStreakDate.Value, today);
            user.BreakStreak();
            user.IncrementStreak(today);
        }
    }
}
