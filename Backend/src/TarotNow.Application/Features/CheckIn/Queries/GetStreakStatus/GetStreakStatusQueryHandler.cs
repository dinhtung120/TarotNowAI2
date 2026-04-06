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

        // 1. Phán Xử Coi Đã Điểm Danh Chưa
        var isCheckedIn = await _checkinRepository.HasCheckedInAsync(user.Id.ToString(), todayString, cancellationToken);

        // 2. Chẩn Đoán Lỗi Lầm Có Làm Gãy Cửa Không
        var isBroken = false;
        long freezeWindowRemainingSeconds = 0;
        bool canBuyFreeze = false;

        // Nếu PreBreakStreak > 0, tức là đã bị gãy
        if (user.PreBreakStreak > 0)
        {
            isBroken = true;

            // Xác định thời gian hết hạn của gói cứu cánh (Cửa sổ 24h tính từ ngay lúc ngày nứt vỡ)
            // Cửa sổ: LastStreakDate + 2 ngày (VD: rút ngày 05, bỏ lỡ 06, vậy ngày 07 là ngày phát hiện đứt -> cửa sổ kết thúc cuối ngày 07 / đầu 08) 
            // Ta cho mua cứu trong vòng đúng {StreakFreezeWindowHours} tiếng kề từ đầu ngày "break".
            
            // Ngày phát hiện gãy vỡ = LastStreakDate.AddDays(2) nếu nó lớn hơn today
            var dateBeforeBreak = user.LastStreakDate ?? todayDate.AddDays(-2); 
            var breakDiscoveryDate = dateBeforeBreak.AddDays(2); 
            
            // Nếu ngày phát hiện đứt vẫn loanh quanh hôm nay thì cửa sổ cứu còn hoạt động
            if (breakDiscoveryDate == todayDate)
            {
                // Cho nó mua cứu. Window tính từ đầu ngày BreakDiscovery + 24h (Tức là cuối ngày BreakDiscovery)
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
