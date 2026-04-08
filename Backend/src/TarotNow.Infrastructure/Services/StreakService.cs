using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Infrastructure.Services;

// Service cập nhật chuỗi streak hằng ngày của người dùng.
public class StreakService : IStreakService
{
    // Repository người dùng để đọc và cập nhật trạng thái streak.
    private readonly IUserRepository _userRepository;
    // Logger theo dõi các nhánh tăng/đứt streak.
    private readonly ILogger<StreakService> _logger;

    /// <summary>
    /// Khởi tạo streak service.
    /// Luồng inject repository và logger để xử lý streak có thể quan sát và kiểm soát lỗi.
    /// </summary>
    public StreakService(IUserRepository userRepository, ILogger<StreakService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <summary>
    /// Tăng streak khi người dùng thực hiện lượt rút hợp lệ trong ngày.
    /// Luồng tránh xử lý lặp trong cùng ngày, áp dụng chuyển trạng thái streak rồi lưu user.
    /// </summary>
    public async Task IncrementStreakOnValidDrawAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            // Edge case: user không tồn tại thì dừng sớm để tránh null reference.
            _logger.LogWarning("[StreakService] Không tìm thấy User {UserId} khi tính Streak.", userId);
            return;
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (IsAlreadyProcessedForToday(user, today))
        {
            // Chặn cập nhật lặp trong cùng ngày để giữ tính đúng của chuỗi streak.
            return;
        }

        try
        {
            // Áp dụng nhánh tăng/đứt streak theo ngày streak gần nhất của user.
            ApplyStreakTransition(user, userId, today);
            await _userRepository.UpdateAsync(user, cancellationToken);
            _logger.LogInformation("[StreakService] ✅ Save Streak thành công cho User {UserId}. CurrentStreak={Streak}, LastDate={Date}.",
                userId, user.CurrentStreak, user.LastStreakDate);
        }
        catch (Exception ex)
        {
            // Nuốt lỗi có log để không làm fail nghiệp vụ chính gọi vào streak.
            _logger.LogError(ex, "[StreakService] Lỗi tèo khi tăng Streak cho User {UserId}.", userId);
        }
    }

    /// <summary>
    /// Kiểm tra user đã được xử lý streak trong ngày hiện tại hay chưa.
    /// Luồng này ngăn một user tăng streak nhiều lần trong cùng business day.
    /// </summary>
    private static bool IsAlreadyProcessedForToday(Domain.Entities.User user, DateOnly today)
        => user.LastStreakDate.HasValue && user.LastStreakDate.Value == today;

    /// <summary>
    /// Áp dụng chuyển trạng thái streak theo mốc ngày gần nhất của user.
    /// Luồng gồm ba nhánh: lần đầu, liên tiếp ngày hôm qua, hoặc đã đứt chuỗi.
    /// </summary>
    private void ApplyStreakTransition(Domain.Entities.User user, Guid userId, DateOnly today)
    {
        if (!user.LastStreakDate.HasValue)
        {
            // User mới chưa có streak thì khởi tạo streak đầu tiên.
            user.IncrementStreak(today);
            _logger.LogInformation("[StreakService] User {UserId} lính mới, bắt đầu Streak = {Streak}.", userId, user.CurrentStreak);
            return;
        }

        var yesterday = today.AddDays(-1);
        if (user.LastStreakDate.Value == yesterday)
        {
            // Check-in liên tiếp ngày liền trước thì tăng chuỗi bình thường.
            user.IncrementStreak(today);
            _logger.LogInformation("[StreakService] User {UserId} streak liền mạch = {Streak}.", userId, user.CurrentStreak);
            return;
        }

        if (user.LastStreakDate.Value < yesterday)
        {
            // Bỏ quá một ngày thì reset chuỗi rồi tính lại từ ngày hôm nay.
            _logger.LogInformation("[StreakService] User {UserId} bị đứt Streak (Từ {LastDate} tới {Today}). Cắt chuỗi.",
                userId, user.LastStreakDate.Value, today);
            user.BreakStreak();
            user.IncrementStreak(today);
        }
    }
}
