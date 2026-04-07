using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

public class DailyCheckInCommandHandler : IRequestHandler<DailyCheckInCommand, DailyCheckInResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IDailyCheckinRepository _checkinRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ISystemConfigSettings _settings;
    private readonly IGamificationService _gamificationService;

    public DailyCheckInCommandHandler(
        IUserRepository userRepository,
        IDailyCheckinRepository checkinRepository,
        IWalletRepository walletRepository,
        ISystemConfigSettings settings,
        IGamificationService gamificationService)
    {
        _userRepository = userRepository;
        _checkinRepository = checkinRepository;
        _walletRepository = walletRepository;
        _settings = settings;
        _gamificationService = gamificationService;
    }

    public async Task<DailyCheckInResult> Handle(DailyCheckInCommand request, CancellationToken cancellationToken)
    {
        
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new NotFoundException($"User {request.UserId} not found");

        
        
        var todayString = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd");

        
        var isCheckedIn = await _checkinRepository.HasCheckedInAsync(request.UserId.ToString(), todayString, cancellationToken);
        if (isCheckedIn)
        {
            return new DailyCheckInResult
            {
                GoldRewarded = 0,
                IsAlreadyCheckedIn = true,
                BusinessDate = todayString,
                CurrentStreak = user.CurrentStreak
            };
        }

        
        var goldAmount = _settings.DailyCheckinGold;

        
        
        await _walletRepository.CreditAsync(
            userId: user.Id,
            currency: CurrencyType.Gold,
            type: TransactionType.DailyCheckin,
            amount: goldAmount,
            description: $"Daily Check-in Reward for {todayString}",
            idempotencyKey: $"checkin_{user.Id}_{todayString}",
            cancellationToken: cancellationToken);

        
        await _checkinRepository.InsertAsync(user.Id.ToString(), todayString, goldAmount, cancellationToken);

        
        
        
        
        await _gamificationService.OnCheckInAsync(user.Id, user.CurrentStreak, cancellationToken);

        
        return new DailyCheckInResult
        {
            GoldRewarded = goldAmount,
            IsAlreadyCheckedIn = false,
            BusinessDate = todayString,
            CurrentStreak = user.CurrentStreak
        };
    }
}
