using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;

// Handler truy vấn trạng thái streak hiện tại của user.
public class GetStreakStatusQueryHandler : IRequestHandler<GetStreakStatusQuery, StreakStatusResult>
{
    // Định dạng ngày business hiển thị cho API response.
    private const string DateFormat = "yyyy-MM-dd";

    // Mốc đầu ngày UTC dùng tính cửa sổ mua freeze.
    private static readonly TimeOnly StartOfDayTime = new(0, 0);

    private readonly IUserRepository _userRepository;
    private readonly IDailyCheckinRepository _checkinRepository;
    private readonly ISystemConfigSettings _settings;

    /// <summary>
    /// Khởi tạo handler get streak status.
    /// Luồng xử lý: nhận repository user/check-in và system settings để tính trạng thái streak + freeze window.
    /// </summary>
    public GetStreakStatusQueryHandler(
        IUserRepository userRepository,
        IDailyCheckinRepository checkinRepository,
        ISystemConfigSettings settings)
    {
        _userRepository = userRepository;
        _checkinRepository = checkinRepository;
        _settings = settings;
    }

    /// <summary>
    /// Xử lý query lấy trạng thái streak.
    /// Luồng xử lý: tải user, kiểm tra check-in hôm nay, tính trạng thái cửa sổ mua freeze, rồi tổng hợp toàn bộ chỉ số streak trả về client.
    /// </summary>
    public async Task<StreakStatusResult> Handle(GetStreakStatusQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            // Edge case: user không tồn tại.
            throw new NotFoundException($"User {request.UserId} not found");
        }

        var todayDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var todayString = todayDate.ToString(DateFormat);
        var isCheckedIn = await _checkinRepository.HasCheckedInAsync(user.Id.ToString(), todayString, cancellationToken);

        var freezeWindowState = ResolveFreezeWindowState(user, todayDate);
        // PreBreakStreak > 0 nghĩa là streak đã bị gãy và chưa xử lý xong.
        var isBroken = user.PreBreakStreak > 0;

        return new StreakStatusResult
        {
            CurrentStreak = user.CurrentStreak,
            LastStreakDate = user.LastStreakDate?.ToString(DateFormat),
            PreBreakStreak = user.PreBreakStreak,
            IsStreakBroken = isBroken,
            FreezePrice = user.CalculateFreezePrice(),
            FreezeWindowRemainingSeconds = freezeWindowState.RemainingSeconds,
            CanBuyFreeze = freezeWindowState.CanBuyFreeze,
            TodayCheckedIn = isCheckedIn,
            ExpMultiplier = user.GetStreakExpMultiplier()
        };
    }

    /// <summary>
    /// Tính trạng thái cửa sổ mua freeze theo thông tin streak hiện tại.
    /// Luồng xử lý: yêu cầu có pre-break streak, xác định ngày phát hiện vỡ chuỗi, tính thời điểm kết thúc cửa sổ và số giây còn lại.
    /// </summary>
    private FreezeWindowState ResolveFreezeWindowState(Domain.Entities.User user, DateOnly todayDate)
    {
        if (user.PreBreakStreak <= 0)
        {
            // Không có streak bị gãy thì không có cửa sổ mua freeze.
            return new FreezeWindowState(false, 0);
        }

        var dateBeforeBreak = user.LastStreakDate ?? todayDate.AddDays(-2);
        var breakDiscoveryDate = dateBeforeBreak.AddDays(2);
        if (breakDiscoveryDate != todayDate)
        {
            // Chỉ cho mua freeze trong ngày phát hiện gãy streak.
            return new FreezeWindowState(false, 0);
        }

        var windowEnd = breakDiscoveryDate
            .ToDateTime(StartOfDayTime)
            .AddHours(_settings.StreakFreezeWindowHours);
        var remainingSeconds = (long)(windowEnd - DateTime.UtcNow).TotalSeconds;
        if (remainingSeconds <= 0)
        {
            // Hết cửa sổ thì khóa khả năng mua freeze.
            return new FreezeWindowState(false, 0);
        }

        return new FreezeWindowState(true, remainingSeconds);
    }

    // Kết quả tính toán cửa sổ mua freeze.
    private readonly record struct FreezeWindowState(bool CanBuyFreeze, long RemainingSeconds);
}
