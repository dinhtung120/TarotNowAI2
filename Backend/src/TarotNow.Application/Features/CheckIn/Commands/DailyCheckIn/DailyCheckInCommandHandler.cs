using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

// Handler xử lý nghiệp vụ check-in hằng ngày.
public class DailyCheckInCommandHandler : IRequestHandler<DailyCheckInCommand, DailyCheckInResult>
{
    // Định dạng business date chuẩn dùng cho check-in theo UTC.
    private const string DateFormat = "yyyy-MM-dd";

    // Prefix idempotency cho giao dịch thưởng check-in.
    private const string CheckinIdempotencyPrefix = "checkin_";
    private const string CheckinLockPrefix = "checkin:lock:";
    private const int CheckinLockRetryAttempts = 8;
    private const int CheckinLockRetryDelayMs = 40;
    private static readonly TimeSpan CheckinLockLease = TimeSpan.FromSeconds(15);

    private readonly IUserRepository _userRepository;
    private readonly IDailyCheckinRepository _checkinRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ICacheService _cacheService;
    private readonly ISystemConfigSettings _settings;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler daily check-in.
    /// Luồng xử lý: nhận repository user/check-in/wallet, settings phần thưởng và service gamification.
    /// </summary>
    public DailyCheckInCommandHandler(
        IUserRepository userRepository,
        IDailyCheckinRepository checkinRepository,
        IWalletRepository walletRepository,
        ICacheService cacheService,
        ISystemConfigSettings settings,
        IDomainEventPublisher domainEventPublisher)
    {
        _userRepository = userRepository;
        _checkinRepository = checkinRepository;
        _walletRepository = walletRepository;
        _cacheService = cacheService;
        _settings = settings;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command check-in hằng ngày.
    /// Luồng xử lý: tải user, kiểm tra đã check-in trong ngày chưa, nếu chưa thì thưởng vàng + ghi log check-in + cập nhật gamification.
    /// </summary>
    public async Task<DailyCheckInResult> Handle(DailyCheckInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            // Edge case: user không tồn tại nên dừng xử lý ngay.
            throw new NotFoundException($"User {request.UserId} not found");
        }

        var todayString = DateOnly.FromDateTime(DateTime.UtcNow).ToString(DateFormat);
        var goldAmount = _settings.DailyCheckinGold;
        var lockContext = CreateCheckinLockContext(user.Id, todayString);
        if (!await AcquireCheckinLockAsync(lockContext.Key, lockContext.Owner, cancellationToken))
        {
            return BuildAlreadyCheckedInResult(todayString, user.CurrentStreak);
        }

        try
        {
            var alreadyCheckedIn = await _checkinRepository.HasCheckedInAsync(
                user.Id.ToString(),
                todayString,
                cancellationToken);
            if (alreadyCheckedIn)
            {
                return BuildAlreadyCheckedInResult(todayString, user.CurrentStreak);
            }

            // Luồng check-in insert-first: credit idempotent trước, sau đó cố gắng insert bản ghi check-in.
            await CreditCheckInRewardAsync(user.Id, todayString, goldAmount, cancellationToken);
            var inserted = await _checkinRepository.TryInsertAsync(user.Id.ToString(), todayString, goldAmount, cancellationToken);
            if (!inserted)
            {
                return BuildAlreadyCheckedInResult(todayString, user.CurrentStreak);
            }

            await PublishCheckinEventsAsync(user.Id, user.CurrentStreak, todayString, goldAmount, cancellationToken);
            return BuildCheckedInResult(todayString, user.CurrentStreak, goldAmount);
        }
        finally
        {
            await _cacheService.ReleaseLockAsync(lockContext.Key, lockContext.Owner, cancellationToken);
        }
    }

    private static (string Key, string Owner) CreateCheckinLockContext(Guid userId, string businessDate)
    {
        return (
            $"{CheckinLockPrefix}{userId:N}:{businessDate}",
            $"{userId:N}:{Guid.NewGuid():N}");
    }

    private async Task<bool> AcquireCheckinLockAsync(string lockKey, string lockOwner, CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < CheckinLockRetryAttempts; attempt++)
        {
            var acquired = await _cacheService.AcquireLockAsync(lockKey, lockOwner, CheckinLockLease, cancellationToken);
            if (acquired)
            {
                return true;
            }

            await Task.Delay(CheckinLockRetryDelayMs, cancellationToken);
        }

        return false;
    }

    private async Task PublishCheckinEventsAsync(
        Guid userId,
        int currentStreak,
        string businessDate,
        long goldAmount,
        CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = userId,
                Currency = CurrencyType.Gold,
                ChangeType = TransactionType.DailyCheckin,
                DeltaAmount = goldAmount,
                ReferenceId = $"{CheckinIdempotencyPrefix}{userId}_{businessDate}"
            },
            cancellationToken);
        await _domainEventPublisher.PublishAsync(
            new TarotNow.Domain.Events.DailyCheckInCompletedDomainEvent
            {
                UserId = userId,
                CurrentStreak = currentStreak,
                BusinessDate = businessDate,
                GoldRewarded = goldAmount
            },
            cancellationToken);
    }

    /// <summary>
    /// Cộng thưởng vàng check-in vào ví user theo idempotency key ổn định.
    /// Luồng xử lý: gọi wallet credit với currency Gold và transaction type DailyCheckin.
    /// </summary>
    private async Task CreditCheckInRewardAsync(
        Guid userId,
        string businessDate,
        long goldAmount,
        CancellationToken cancellationToken)
    {
        await _walletRepository.CreditAsync(
            userId: userId,
            currency: CurrencyType.Gold,
            type: TransactionType.DailyCheckin,
            amount: goldAmount,
            description: $"Daily Check-in Reward for {businessDate}",
            idempotencyKey: $"{CheckinIdempotencyPrefix}{userId}_{businessDate}",
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Dựng kết quả khi user đã check-in trước đó trong ngày.
    /// Luồng xử lý: trả reward bằng 0 và cờ IsAlreadyCheckedIn=true để client hiển thị đúng trạng thái.
    /// </summary>
    private static DailyCheckInResult BuildAlreadyCheckedInResult(string businessDate, int currentStreak)
    {
        return new DailyCheckInResult
        {
            GoldRewarded = 0,
            IsAlreadyCheckedIn = true,
            BusinessDate = businessDate,
            CurrentStreak = currentStreak
        };
    }

    /// <summary>
    /// Dựng kết quả khi check-in thành công và đã cộng thưởng.
    /// Luồng xử lý: trả số vàng được thưởng, business date và current streak hiện tại.
    /// </summary>
    private static DailyCheckInResult BuildCheckedInResult(
        string businessDate,
        int currentStreak,
        long goldAmount)
    {
        return new DailyCheckInResult
        {
            GoldRewarded = goldAmount,
            IsAlreadyCheckedIn = false,
            BusinessDate = businessDate,
            CurrentStreak = currentStreak
        };
    }
}
