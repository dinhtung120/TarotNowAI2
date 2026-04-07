using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.CheckIn.Commands.PurchaseFreeze;

public class PurchaseStreakFreezeCommandHandler : IRequestHandler<PurchaseStreakFreezeCommand, PurchaseStreakFreezeResult>
{
    private const string FreezeIdempotencyPrefix = "freeze_";
    private static readonly TimeOnly StartOfDayTime = new(0, 0);

    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ISystemConfigSettings _settings;

    public PurchaseStreakFreezeCommandHandler(
        IUserRepository userRepository,
        IWalletRepository walletRepository,
        ISystemConfigSettings settings)
    {
        _userRepository = userRepository;
        _walletRepository = walletRepository;
        _settings = settings;
    }

    public async Task<PurchaseStreakFreezeResult> Handle(PurchaseStreakFreezeCommand request, CancellationToken cancellationToken)
    {
        var idempotencyKey = ValidateAndNormalizeIdempotencyKey(request.IdempotencyKey);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new NotFoundException($"User {request.UserId} not found");

        EnsureUserHasBrokenStreak(user.PreBreakStreak);
        EnsurePurchaseWindowIsValid(user.LastStreakDate, _settings.StreakFreezeWindowHours);

        var priceDiamond = user.CalculateFreezePrice();
        await DebitDiamondAsync(user, priceDiamond, idempotencyKey, cancellationToken);
        user.RestoreStreak();
        await _userRepository.UpdateAsync(user, cancellationToken);

        return new PurchaseStreakFreezeResult
        {
            Success = true,
            RestoredStreak = user.CurrentStreak,
            DiamondCost = priceDiamond
        };
    }

    private static string ValidateAndNormalizeIdempotencyKey(string? idempotencyKey)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new ArgumentException(
                "Thiếu IdempotencyKey để giao dịch Kim Cương.",
                nameof(idempotencyKey));
        }

        return idempotencyKey.Trim();
    }

    private static void EnsureUserHasBrokenStreak(int preBreakStreak)
    {
        if (preBreakStreak > 0) return;
        throw new InvalidOperationException("Bạn không có chuỗi Streak bị gãy nào để phải tốn tiền phục hồi.");
    }

    private static void EnsurePurchaseWindowIsValid(DateOnly? lastStreakDate, int freezeWindowHours)
    {
        var todayDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateBeforeBreak = lastStreakDate ?? todayDate.AddDays(-2);
        var breakDiscoveryDate = dateBeforeBreak.AddDays(2);
        if (breakDiscoveryDate != todayDate)
        {
            throw new InvalidOperationException(
                "Đã quá hạn để đóng băng/phục hồi Streak (Chỉ có quyền mua trong ngày phát hiện vỡ chuỗi).");
        }

        var windowEnd = breakDiscoveryDate.ToDateTime(StartOfDayTime).AddHours(freezeWindowHours);
        if (DateTime.UtcNow <= windowEnd) return;

        throw new InvalidOperationException(
            $"Đã quá cửa sổ {freezeWindowHours} tiếng kể từ lúc bị gãy. Không thể phục hồi nữa.");
    }

    private async Task DebitDiamondAsync(
        Domain.Entities.User user,
        long priceDiamond,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        await _walletRepository.DebitAsync(
            userId: user.Id,
            currency: CurrencyType.Diamond,
            type: TransactionType.StreakFreezeCost,
            amount: priceDiamond,
            description: $"Mua Lệnh Hồi Sinh Chuỗi Streak {user.PreBreakStreak} Ngày.",
            idempotencyKey: FreezeIdempotencyPrefix + idempotencyKey,
            cancellationToken: cancellationToken);
    }
}
