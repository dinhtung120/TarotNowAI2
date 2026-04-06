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
        // 1. Tải Hộ Khẩu User
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new NotFoundException($"User {request.UserId} not found");

        // 2. Chốt Lịch Kế Toán (Business Date).
        // Lấy DateOnly từ giờ UTC hiện tại.
        var todayString = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd");

        // 3. Kiểm sổ xem hôm nay bấm chưa. (Idempotent: Ngăn user bấm nhiều lần ăn gian).
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

        // 4. Nếu chưa bấm thì phát Vàng.
        var goldAmount = _settings.DailyCheckinGold;

        // Bơm tiền trực tiếp vô ví 
        // Bắt IdempotentKey từ ví để lỡ sụp nguồn thì không cộng đôi.
        await _walletRepository.CreditAsync(
            userId: user.Id,
            currency: CurrencyType.Gold,
            type: TransactionType.DailyCheckin,
            amount: goldAmount,
            description: $"Daily Check-in Reward for {todayString}",
            idempotencyKey: $"checkin_{user.Id}_{todayString}",
            cancellationToken: cancellationToken);

        // 5. Thắp Hương Trữ Lịch Sử Nhận (Gọi vào Repo để Repo tự tạo Document).
        await _checkinRepository.InsertAsync(user.Id.ToString(), todayString, goldAmount, cancellationToken);

        // (Tuỳ chọn: Nếu push notification ví đã lên tiền thì IWalletRepository tự bắn hoặc PushSvc bắn).
        
        // --- GAMIFICATION PHASE 5.3 ---
        // Báo cho Gamification biết user vừa điểm danh để chạy Nhiệm Vụ (Quest) & Leaderboard.
        await _gamificationService.OnCheckInAsync(user.Id, user.CurrentStreak, cancellationToken);

        // 6. Trả Kết Quả Về Vinh Quang
        return new DailyCheckInResult
        {
            GoldRewarded = goldAmount,
            IsAlreadyCheckedIn = false,
            BusinessDate = todayString,
            CurrentStreak = user.CurrentStreak
        };
    }
}
