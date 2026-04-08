using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.CheckIn.Commands.DailyCheckIn;

// Handler xử lý nghiệp vụ check-in hằng ngày.
public class DailyCheckInCommandHandler : IRequestHandler<DailyCheckInCommand, DailyCheckInResult>
{
    // Định dạng business date chuẩn dùng cho check-in theo UTC.
    private const string DateFormat = "yyyy-MM-dd";

    // Prefix idempotency cho giao dịch thưởng check-in.
    private const string CheckinIdempotencyPrefix = "checkin_";

    private readonly IUserRepository _userRepository;
    private readonly IDailyCheckinRepository _checkinRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ISystemConfigSettings _settings;
    private readonly IGamificationService _gamificationService;

    /// <summary>
    /// Khởi tạo handler daily check-in.
    /// Luồng xử lý: nhận repository user/check-in/wallet, settings phần thưởng và service gamification.
    /// </summary>
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
        var isCheckedIn = await _checkinRepository.HasCheckedInAsync(request.UserId.ToString(), todayString, cancellationToken);
        if (isCheckedIn)
        {
            // User đã check-in hôm nay thì trả kết quả idempotent, không cộng thưởng lại.
            return BuildAlreadyCheckedInResult(todayString, user.CurrentStreak);
        }

        var goldAmount = _settings.DailyCheckinGold;
        // Luồng check-in thành công: cộng vàng, ghi bản ghi check-in và kích hoạt gamification.
        await CreditCheckInRewardAsync(user.Id, todayString, goldAmount, cancellationToken);
        await _checkinRepository.InsertAsync(user.Id.ToString(), todayString, goldAmount, cancellationToken);
        await _gamificationService.OnCheckInAsync(user.Id, user.CurrentStreak, cancellationToken);

        return BuildCheckedInResult(todayString, user.CurrentStreak, goldAmount);
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
