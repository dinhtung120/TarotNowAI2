using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;

public class GetStreakStatusQueryHandler : IRequestHandler<GetStreakStatusQuery, StreakStatusResult>
{
    private const string DateFormat = "yyyy-MM-dd";
    private static readonly TimeOnly StartOfDayTime = new(0, 0);

    private readonly IUserRepository _userRepository;
    private readonly IDailyCheckinRepository _checkinRepository;
    private readonly ISystemConfigSettings _settings;

    public GetStreakStatusQueryHandler(
        IUserRepository userRepository,
        IDailyCheckinRepository checkinRepository,
        ISystemConfigSettings settings)
    {
        _userRepository = userRepository;
        _checkinRepository = checkinRepository;
        _settings = settings;
    }

    public async Task<StreakStatusResult> Handle(GetStreakStatusQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new NotFoundException($"User {request.UserId} not found");

        var todayDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var todayString = todayDate.ToString(DateFormat);
        var isCheckedIn = await _checkinRepository.HasCheckedInAsync(user.Id.ToString(), todayString, cancellationToken);

        var freezeWindowState = ResolveFreezeWindowState(user, todayDate);
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

    private FreezeWindowState ResolveFreezeWindowState(Domain.Entities.User user, DateOnly todayDate)
    {
        if (user.PreBreakStreak <= 0) return new FreezeWindowState(false, 0);

        var dateBeforeBreak = user.LastStreakDate ?? todayDate.AddDays(-2);
        var breakDiscoveryDate = dateBeforeBreak.AddDays(2);
        if (breakDiscoveryDate != todayDate) return new FreezeWindowState(false, 0);

        var windowEnd = breakDiscoveryDate
            .ToDateTime(StartOfDayTime)
            .AddHours(_settings.StreakFreezeWindowHours);
        var remainingSeconds = (long)(windowEnd - DateTime.UtcNow).TotalSeconds;
        if (remainingSeconds <= 0) return new FreezeWindowState(false, 0);

        return new FreezeWindowState(true, remainingSeconds);
    }

    private readonly record struct FreezeWindowState(bool CanBuyFreeze, long RemainingSeconds);
}
