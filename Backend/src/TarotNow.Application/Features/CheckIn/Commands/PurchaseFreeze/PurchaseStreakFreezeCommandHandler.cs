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
        if (string.IsNullOrWhiteSpace(request.IdempotencyKey))
            throw new ArgumentException("Thiếu IdempotencyKey để giao dịch Kim Cương.", nameof(request.IdempotencyKey));

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new NotFoundException($"User {request.UserId} not found");

        
        if (user.PreBreakStreak <= 0)
        {
            throw new InvalidOperationException("Bạn không có chuỗi Streak bị gãy nào để phải tốn tiền phục hồi.");
        }

        
        var todayDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var dateBeforeBreak = user.LastStreakDate ?? todayDate.AddDays(-2); 
        var breakDiscoveryDate = dateBeforeBreak.AddDays(2);
        
        if (breakDiscoveryDate != todayDate)
        {
             
             throw new InvalidOperationException("Đã quá hạn để đóng băng/phục hồi Streak (Chỉ có quyền mua trong ngày phát hiện vỡ chuỗi).");
        }
        
        var windowEnd = breakDiscoveryDate.ToDateTime(new TimeOnly(0, 0)).AddHours(_settings.StreakFreezeWindowHours);
        if (DateTime.UtcNow > windowEnd)
        {
            throw new InvalidOperationException($"Đã quá cửa sổ {_settings.StreakFreezeWindowHours} tiếng kể từ lúc bị gãy. Không thể phục hồi nữa.");
        }

        
        var priceDiamond = user.CalculateFreezePrice();

        
        
        await _walletRepository.DebitAsync(
            userId: user.Id,
            currency: CurrencyType.Diamond,
            type: TransactionType.StreakFreezeCost,
            amount: priceDiamond,
            description: $"Mua Lệnh Hồi Sinh Chuỗi Streak {user.PreBreakStreak} Ngày.",
            idempotencyKey: $"freeze_{request.IdempotencyKey}",
            cancellationToken: cancellationToken);

        
        user.RestoreStreak();
        
        
        await _userRepository.UpdateAsync(user, cancellationToken);

        
        return new PurchaseStreakFreezeResult
        {
            Success = true,
            RestoredStreak = user.CurrentStreak,
            DiamondCost = priceDiamond
        };
    }
}
