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
        var todayString = todayDate.ToString("yyyy-MM-dd");

        
        var isCheckedIn = await _checkinRepository.HasCheckedInAsync(user.Id.ToString(), todayString, cancellationToken);

        
        var isBroken = false;
        long freezeWindowRemainingSeconds = 0;
        bool canBuyFreeze = false;

        
        if (user.PreBreakStreak > 0)
        {
            isBroken = true;

            
            
            
            
            
            var dateBeforeBreak = user.LastStreakDate ?? todayDate.AddDays(-2); 
            var breakDiscoveryDate = dateBeforeBreak.AddDays(2); 
            
            
            if (breakDiscoveryDate == todayDate)
            {
                
                var windowEnd = breakDiscoveryDate.ToDateTime(new TimeOnly(0, 0)).AddHours(_settings.StreakFreezeWindowHours);
                var now = DateTime.UtcNow;

                var diff = windowEnd - now;
                if (diff.TotalSeconds > 0)
                {
                    canBuyFreeze = true;
                    freezeWindowRemainingSeconds = (long)diff.TotalSeconds;
                }
            }
        }

        return new StreakStatusResult
        {
            CurrentStreak = user.CurrentStreak,
            LastStreakDate = user.LastStreakDate?.ToString("yyyy-MM-dd"),
            PreBreakStreak = user.PreBreakStreak,
            IsStreakBroken = isBroken,
            FreezePrice = user.CalculateFreezePrice(),
            FreezeWindowRemainingSeconds = freezeWindowRemainingSeconds,
            CanBuyFreeze = canBuyFreeze,
            TodayCheckedIn = isCheckedIn,
            ExpMultiplier = user.GetStreakExpMultiplier()
        };
    }
}
